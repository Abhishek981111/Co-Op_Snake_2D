using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SnakeController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject bodySegmentPrefab;
    public float initialBodySegmentDistance;
    private bool justAte = false;
    private bool isAlive = true;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;           //TBC

    public float growthAmount;    //adjust value for flexible growth
    public float decreaseAmount;  //adjust value for flexible decrease

    private Vector2 currentDirection;
    private Rigidbody2D rb;
    private bool canChangeDirection = true;
    private KeyCode[] playerKeys;
    private List<Transform> bodySegments = new List<Transform>();

    private Vector2 ScreenBounds;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    public GameObject[] powerUps; // Array of power-up prefabs

    public GameObject shieldPrefab;
    public GameObject scoreBoostPrefab;
    public GameObject speedUpPrefab;

    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;
    //private bool isSpeedUpActive = false;

    private float powerUpCooldown = 10f;
    private float timeSinceLastPowerUp = 0f;
    private float powerUpDuration = 8f;

    public UIManager uiManager;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Vector2.right; // Initial direction
    
        // Calculate screen bounds based on the camera's orthographic size and aspect ratio
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        ScreenBounds = new Vector2(cameraWidth, cameraHeight);

        // Set player keys based on the GameObject's tag
        if (gameObject.CompareTag("Player1"))
            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
        else if (gameObject.CompareTag("Player2"))
            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

        // Start snake movement immediately
        rb.velocity = currentDirection * moveSpeed;

        timeSinceLastPowerUp = powerUpCooldown;

        shieldPrefab.SetActive(false);
        scoreBoostPrefab.SetActive(false);
        speedUpPrefab.SetActive(false);

        StartCoroutine(SpawnPowerUpsRoutine());
        SoundManager.Instance.Play(Sounds.Music);
    }

    private void Update()
    {
        timeSinceLastPowerUp += Time.deltaTime;

        if (timeSinceLastPowerUp >= powerUpCooldown)
        {
            
            //ActivateRandomPowerUp();
            timeSinceLastPowerUp = 0.0f;
        }

        if(isAlive)
        {
            HandleInput();
        }
    }

    private void FixedUpdate()
    {
        // Move the snake automatically in its current direction
        MoveSnake(currentDirection);
    }

    private void LateUpdate()
    {
        //Using Late Update because moving snake is giving me some flickering issues when snake tries eating food
        if (justAte)
        {
            justAte = false;
            Vector2 prevPosition = rb.position;

            foreach (Transform segment in bodySegments)
            {
                segment.position = prevPosition - (Vector2)transform.up * initialBodySegmentDistance;
                prevPosition = segment.position;
            }
        }
    }

    private void HandleInput()
    {
        if (!canChangeDirection)
            return;

        if (Input.GetKeyDown(playerKeys[0]) && currentDirection != Vector2.down)
            {
                ChangeDirection(Vector2.up);
            }
        else if (Input.GetKeyDown(playerKeys[1]) && currentDirection != Vector2.up)
            {
                ChangeDirection(Vector2.down);
            }
        else if (Input.GetKeyDown(playerKeys[2]) && currentDirection != Vector2.right)
            {
                ChangeDirection(Vector2.left);
            }
        else if (Input.GetKeyDown(playerKeys[3]) && currentDirection != Vector2.left)
            {
                ChangeDirection(Vector2.right);
            }
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        canChangeDirection = false;
        currentDirection = newDirection;
        UpdateHeadSprite();                           //TBC
        Invoke("EnableChangeDirection", 0.2f); // Delay to prevent rapid direction changes
    }

    private void EnableChangeDirection()
    {
        canChangeDirection = true;
    }

    private void MoveSnake(Vector2 direction)
    {
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        //Check for self bite
        if(IsSelfBite(newPosition))
        {
            Die();
            return;
        }

        //screen Wrapping
        if(newPosition.x < -ScreenBounds.x)
        {
            newPosition.x = ScreenBounds.x;
        }
        else if(newPosition.x > ScreenBounds.x)
        {
            newPosition.x = -ScreenBounds.x;
        }
        else if(newPosition.y < -ScreenBounds.y)
        {
            newPosition.y = ScreenBounds.y;
        }
        else if(newPosition.y > ScreenBounds.y)
        {
            newPosition.y = -ScreenBounds.y;
        }
        
        rb.MovePosition(newPosition);

        //Move the body segments
        MoveBodySegments();
    }

    // TBC
    private void UpdateHeadSprite()
    {
        // Change snake head sprite based on movement direction
        if (currentDirection == Vector2.up)
        {
            GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (currentDirection == Vector2.down)
        {
            GetComponent<SpriteRenderer>().sprite = downSprite;
        }
        else if (currentDirection == Vector2.left)
        {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else if (currentDirection == Vector2.right)
        {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("GrowthFood"))
        {
            SoundManager.Instance.Play(Sounds.FoodEating);
            Destroy(collision.gameObject);
            GrowSnake();
            int scoreMultiplier = isScoreBoostActive ? 2 : 1;
            IncreaseScore(10 * scoreMultiplier);  
        }
        else if(collision.CompareTag("DecreaseFood"))
        {
            SoundManager.Instance.Play(Sounds.FoodEating);
            Destroy(collision.gameObject);
            DecreaseSnake();
            DecreaseScore(5);
        }
        else if(collision.CompareTag("ShieldPowerUp"))
        {
            SoundManager.Instance.Play(Sounds.PowerUpsEating);
            Destroy(collision.gameObject);
            ActivateShield();
        }
        else if(collision.CompareTag("ScoreBoostPowerUp"))
        {
            SoundManager.Instance.Play(Sounds.PowerUpsEating);
            Destroy(collision.gameObject);
            ActivateScoreBoost();
        }
        else if(collision.CompareTag("SpeedUpPowerUp"))
        {
            SoundManager.Instance.Play(Sounds.PowerUpsEating);
            Destroy(collision.gameObject);
            ActivateSpeedUp();
        }
        //also for collision with other snake
        else if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            SnakeController otherSnake = collision.GetComponent<SnakeController>();
            if(otherSnake != null)
            {
                //Debug.Log("Collision with other snake detected!!!");
                if (isShieldActive || otherSnake.isShieldActive)
                {
                    Debug.Log("Snake shield is active");
                }
                else 
                {
                    Die();
                    otherSnake.Die();
                }
            }
        }
    }

    private IEnumerator SpawnPowerUpsRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 15f));    
            //GameObject[] powerUpPrefabs = { shieldPrefab, scoreBoostPrefab, speedUpPrefab };
            int randomIndex = UnityEngine.Random.Range(0, powerUps.Length);
            GameObject randomPowerUp = powerUps[randomIndex];
            Vector3 spawnPosition = GetRandomSpawnPosition(); 

            //Instantiate the power-up prefab and enable it
            GameObject spawnedPowerUp = Instantiate(randomPowerUp, spawnPosition, Quaternion.identity);
            spawnedPowerUp.SetActive(true);

            //timer to destroy the power-up if not collected
            StartCoroutine(DestroyPowerUpAfterTime(spawnedPowerUp));
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = UnityEngine.Random.Range(-ScreenBounds.x, ScreenBounds.x);
        float randomY = UnityEngine.Random.Range(-ScreenBounds.y, ScreenBounds.y);
        return new Vector3(randomX, randomY, 0f);
    }

    private void ActivateShield()
    {
        isShieldActive = true; 
        StartCoroutine(DeactivatePowerUp(shieldPrefab));
    }

    private void ActivateScoreBoost()
    {
        isScoreBoostActive = true;
        // Implement logic to increase the score for a duration.
        StartCoroutine(DeactivatePowerUp(scoreBoostPrefab));
    }

    private void ActivateSpeedUp()
    {
        //isSpeedUpActive = true;
        moveSpeed *= 2.25f;  //Increase the snake's speed by some factor
        StartCoroutine(DeactivatePowerUp(speedUpPrefab));
    }

    private IEnumerator DeactivatePowerUp(GameObject powerUpPrefab)
    {
        // Activate the power-up object.
        powerUpPrefab.SetActive(true);

        // Wait for the power-up duration.
        yield return new WaitForSeconds(powerUpDuration);

        // Deactivate the power-up object.
        powerUpPrefab.SetActive(false);

        if(powerUpPrefab == shieldPrefab)
        {
            isShieldActive = false;
        }
        else if(powerUpPrefab == speedUpPrefab)
        {
            //isSpeedUpActive = false;
            moveSpeed /= 2.25f;   //Reset the snake's speed.
        }
        else if(powerUpPrefab == scoreBoostPrefab)
        {
            isScoreBoostActive = false;
        }
    }

    private IEnumerator DestroyPowerUpAfterTime(GameObject powerUp)
    {
        yield return new WaitForSeconds(10f);

        // Checking if the power-up still exists and hasn't been collected
        if(powerUp != null)
        {
            Destroy(powerUp);
        }
    }

    private bool IsSelfBite(Vector2 position)
    {
        foreach(Transform segment in bodySegments)
        {
            if((Vector2)segment.position == position)
            {
                return true;
            }
        }
        return false;
    }

    private void Die()
    {
        isAlive = false;
        rb.velocity = Vector2.zero;
        Debug.Log("Snake died!");
        SoundManager.Instance.Play(Sounds.SnakeDeath);
        this.enabled = false;

        //game over logic 
        if (uiManager != null)
        {
            //Notify UIManager about snake's death
            uiManager.HandleSnakeDeath(gameObject.tag);
        }
    }

    private void MoveBodySegments()
    {
        Vector2 prevPosition = rb.position;
        
        foreach (Transform segment in bodySegments)
        {
            Vector2 tempPosition = segment.position;
            segment.position = prevPosition;
            prevPosition = tempPosition;
        }
    }

    private void UpdateBodySegmentPositions(float amount)
    {
        Vector2 prevPosition = rb.position;

        foreach (Transform segment in bodySegments)
        {
            segment.position = prevPosition - (Vector2)transform.up * amount;
            prevPosition = segment.position;
        }
    }

    private void GrowSnake()
    {
        GameObject newSegment = Instantiate(bodySegmentPrefab);
        bodySegments.Add(newSegment.transform);
        justAte = true;

        UpdateBodySegmentPositions(growthAmount);
    }


    private void DecreaseSnake()
    {
        if (bodySegments.Count > 0)
        {
            int segmentsToRemove = Mathf.Min(bodySegments.Count, Mathf.CeilToInt(decreaseAmount));
            
            for (int i = 0; i < segmentsToRemove; i++)
            {
                Transform lastSegment = bodySegments[bodySegments.Count - 1];
                bodySegments.Remove(lastSegment);
                Destroy(lastSegment.gameObject);
            }
        } 
    }

    public int GetSnakeSize()
    {
        return bodySegments.Count + 1;  //Added 1 for the head
    }

    private void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    private void DecreaseScore(int amount)
    {
        score -= amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}