using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject bodySegmentPrefab;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Vector2 currentDirection;
    private Rigidbody2D rb;
    private bool canChangeDirection = true;
    private KeyCode[] playerKeys;
    private List<Transform> bodySegments = new List<Transform>();

    private Vector2 ScreenBounds;

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
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        // Move the snake automatically in its current direction
        MoveSnake(currentDirection);
    }

    private void HandleInput()
    {
        if (!canChangeDirection)
            return;

        if (Input.GetKeyDown(playerKeys[0]) && currentDirection != Vector2.down)
            {ChangeDirection(Vector2.up);}
        else if (Input.GetKeyDown(playerKeys[1]) && currentDirection != Vector2.up)
            {ChangeDirection(Vector2.down);}
        else if (Input.GetKeyDown(playerKeys[2]) && currentDirection != Vector2.right)
            {ChangeDirection(Vector2.left);}
        else if (Input.GetKeyDown(playerKeys[3]) && currentDirection != Vector2.left)
            {ChangeDirection(Vector2.right);}
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        canChangeDirection = false;
        currentDirection = newDirection;
        UpdateHeadSprite();
        Invoke("EnableChangeDirection", 0.2f); // Delay to prevent rapid direction changes
    }

    private void EnableChangeDirection()
    {
        canChangeDirection = true;
    }

    private void MoveSnake(Vector2 direction)
    {
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

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
        //one position forward each frame, and add a new segment at head's previous location
        if(bodySegments.Count > 0)
        {
            for(int i = bodySegments.Count - 1; i > 0; i--)
            {
                bodySegments[i].position = bodySegments[i - 1].position;
            }
            bodySegments[0].position = rb.position;
        }
    }

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
        }
        else if(collision.CompareTag("DecreaseFood"))
        {
            Destroy(collision.gameObject);
            DecreaseSnake();
        }
        //also for collision with other snake
    }

    private void GrowSnake()
    {
        // Create a new segment and add it to the bodySegments list
        GameObject newSegment = Instantiate(bodySegmentPrefab);

        // If there are existing body segments, position the new segment based on the last segment's position
        if (bodySegments.Count > 0)
        {
            newSegment.transform.position = bodySegments[bodySegments.Count - 1].position;
        }
        else
        {
            // If no segments exist yet, position the new segment at the snake's current position
            newSegment.transform.position = transform.position;
        }

        SpriteRenderer segmentRenderer = bodySegmentPrefab.GetComponent<SpriteRenderer>();
        newSegment.transform.localScale =  segmentRenderer.bounds.size;
        bodySegments.Add(newSegment.transform);

        
        // // Create a new segment and add it to the bodySegments list
        // GameObject newSegment = Instantiate(bodySegmentPrefab);
    
        // // Determine the position for the new segment based on the snake's current tail position
        // Vector3 tailPosition = bodySegments[bodySegments.Count - 1].position;
        // newSegment.transform.position = tailPosition;

        // // Set the scale of the new segment to match the desired size
        // newSegment.transform.localScale = Vector3.one;

        // bodySegments.Add(newSegment.transform);
    }


    private void DecreaseSnake()
    {
        if(bodySegments.Count > 1)
        {
            Destroy(bodySegments[bodySegments.Count - 1].gameObject);
            bodySegments.RemoveAt(bodySegments.Count - 1);
        }
    }
    
}