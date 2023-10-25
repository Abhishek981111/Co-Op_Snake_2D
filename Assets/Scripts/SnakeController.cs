using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

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
    private int score = 0;
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

        //Find TextMeshPro Text component
        //scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
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
            Destroy(collision.gameObject);
            GrowSnake();
            IncreaseScore(10);
        }
        else if(collision.CompareTag("DecreaseFood"))
        {
            Destroy(collision.gameObject);
            DecreaseSnake();
            DecreaseScore(5);
        }
        //also for collision with other snake
        else if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            SnakeController otherSnake = collision.GetComponent<SnakeController>();

            if (otherSnake != null)
            {
                Die();
                otherSnake.Die();
            }
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
        //add game over logic 
        Debug.Log("Snake died!");
    }

    private void MoveBodySegments()
    {
        Vector2 prevPosition = rb.position;
        // Transform lastSegment = null;

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