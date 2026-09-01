using UnityEngine;

public class JoeController : MonoBehaviour
{

    private int health;
    public int maxHealth;
    private Rigidbody2D rb;
    private Vector2 playerInput;

    public GameObject facingIndicator;
    //Horizontal Movement
    [Header("Horizontal Variables")]
    public float hAccelTime;
    public float hDecelTime;
    public float hMaxSpeed;
    private float hAccel;
    private float hDecel;
    public float turnFriction;
    public bool useTF;
    public Vector2 goalVelo;
    public int facingDirection; // -1 is left, 1 is right

    public LayerMask groundLayer;

    float hInput;
    [Header("Jump Variables")]
    public float jumpSpeed;
    public float grav;
    private bool canJump;
    private bool tryJump;
    private bool useJump;
    private bool queueJump;
    public float queueJumpBuffer;
    public float coyoteTimeBuffer;
    public bool canCoyote;
    public bool wasOnGroundLastFrame;
    private bool isGrounded;
    public float coyoteStartTime;
    public float coyoteTime;
    public float maxFallSpeed;
    public float wallJumpCount;
    public bool hasWJCharge;


    [Header("Wall Slide/Jump Variables")]
    public float wallSlideSpeed;
    private bool isOnWall;
    private bool wasOnWallLastFrame;
    private bool isAgainstWall;
    public bool canWallJump;
    public float wallSlideMult; // Gravity * mult (mult should be between 0 and 1)
    public int wallDirection; // -1 is left, 1 is right
    public float pushOffMult; //Max speed  mult (mult should be between 0 and 1)
    private bool canSpiderman; //Coyote jump for for wall jumping (aka window of time you can wall jump after looking away from wall)
    private float spidermanTime;
    public float spidermanBufferTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canJump = true;

        rb = GetComponent<Rigidbody2D>();

        hAccel = hMaxSpeed / hAccelTime;

        hDecel = hMaxSpeed / hDecelTime;

        health = maxHealth;

        facingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                playerInput.x = 0f;
            }

            else if (Input.GetKey(KeyCode.A))
            {
                playerInput.x = -1;
                facingDirection = -1;
            }

            else
            {
                playerInput.x = 1f;
                facingDirection = 1;

            }
        }

        else
        {
            playerInput.x = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            tryJump = true;
          
            Debug.Log("TRY JUMP!");

        }

        if (canCoyote)
        {
           coyoteTime += Time.deltaTime;

            if (coyoteTime > coyoteTimeBuffer)
            {
                canCoyote = false;
            }
        }

        if (canSpiderman)
        {
            spidermanTime += Time.deltaTime;

            if (spidermanTime > spidermanBufferTime)
            {
                canSpiderman = false;
            }
        }






    }

    private void FixedUpdate()
    {
        GroundMovement();
        CheckForwardWall();

        wasOnWallLastFrame = isAgainstWall;
        wasOnGroundLastFrame = isGrounded;

        facingIndicator.transform.position = rb.position + new Vector2(facingDirection * 2, 0f);
        isGrounded = GroundCheck();
        isAgainstWall = CheckForwardWall();

        if (!isGrounded && wasOnGroundLastFrame)
        {
            canCoyote = true;
            coyoteTime = Time.deltaTime;

        }

        if (!isAgainstWall && wasOnWallLastFrame)
        {
            canSpiderman = true;
            spidermanTime = Time.deltaTime;
        }
        
        if (tryJump)
        {
            TryJump(isGrounded);
        }
       
        if (!isGrounded)
        {
            if (isAgainstWall && rb.linearVelocityY < 0 && playerInput.x != 0)
            {
                rb.linearVelocityY -= grav * wallSlideMult * Time.deltaTime;
            }
            else
            {
                rb.linearVelocityY -= grav * Time.deltaTime;
            } 
        }
        else
        {
            hasWJCharge = true;

            if (queueJump)
            {
                UseJump();
                queueJump = false;
            }
               
        }

        if (rb.position.y < -5f)
        {
            rb.position = new Vector2(0f, 0f);
        }

        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocityY = maxFallSpeed;
        }
    }
    
        
    private void TryJump(bool onGround)
    {
        tryJump = false;

        RaycastHit2D hit = Physics2D.BoxCast(rb.position, Vector2.one, 0f, Vector2.down, queueJumpBuffer, groundLayer);

        if (onGround)
        {
            UseJump();
        }

        else if (canCoyote && rb.linearVelocityY < 0)
        {
            Debug.Log("YOTED");
            canCoyote = false;
            UseJump();
        }

        else if (isAgainstWall && hasWJCharge)
        {
            UseWallJump();
        }

        else if (canSpiderman && rb.linearVelocityY < 0)
        {
            Debug.Log("SPIDERMAN");
            canSpiderman = false;
            UseWallJump();
        }


        else if (!onGround && hit && rb.linearVelocityY < 0)
        {
            queueJump = true;
            Debug.Log("QUEUED");
        }

       

       
    }

    private void UseJump()
    {
        rb.linearVelocityY = jumpSpeed;
    }

    public bool CheckForwardWall()
    {
        RaycastHit2D wallHit = Physics2D.BoxCast(rb.position, new Vector2(1f, 0.8f), 0f, new Vector2(facingDirection, 0f), 0.05f,groundLayer);

        if (wallHit)
        {
            // Debug.Log("HIT WALL");
            wallDirection = facingDirection;
            return true;
            
        }
        else
        {
            return false;
        }
    }

    private void UseWallJump()
    {
        rb.linearVelocityY = jumpSpeed;
        rb.linearVelocityX = hMaxSpeed * pushOffMult;
        hasWJCharge = false;
    }

    private void GroundMovement()
    {
        if (playerInput.x != 0)
        {
            if (useTF && Mathf.Sign(playerInput.x) != Mathf.Sign(rb.linearVelocityX))
            {
                rb.linearVelocityX += playerInput.x * turnFriction * Time.deltaTime;
            }

            rb.linearVelocityX += hAccel * playerInput.x * Time.deltaTime;
        }

        else
        {
            float frictionDirection = -Mathf.Sign(rb.linearVelocityX);
            //Debug.Log(frictionDirection);


            if (Mathf.Abs(rb.linearVelocityX) < 1f)
            {
                rb.linearVelocityX = 0;
            }
            else
            {
                rb.linearVelocityX += hDecel * frictionDirection * Time.deltaTime;
            }
        }

        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -hMaxSpeed, hMaxSpeed);

       // Debug.Log(rb.linearVelocityX);
    }

    public bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(rb.position, Vector2.one, 0f, Vector2.down, 0.05f, groundLayer);

        if (hit)
        {
            //Debug.Log("GROUNDED");
            return true;
        }

        else
        {
            return false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Debug.Log("OUCH!");
            health--;

            if (health == 0)
            {
                Debug.Log("Ya ded");
            }
        }
    }
}
