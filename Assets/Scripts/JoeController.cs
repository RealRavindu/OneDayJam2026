using UnityEngine;

public class JoeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 playerInput;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canJump = true;

        rb = GetComponent<Rigidbody2D>();

        hAccel = hMaxSpeed / hAccelTime;

        hDecel = hMaxSpeed / hDecelTime;
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
            }

            else
            {
                playerInput.x = 1f;

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


    }

    private void FixedUpdate()
    {
        GroundMovement();
        wasOnGroundLastFrame = isGrounded;

        isGrounded = GroundCheck();

        if (!isGrounded && wasOnGroundLastFrame)
        {
            canCoyote = true;
            coyoteTime = Time.deltaTime;

        }

        
        if (tryJump)
        {
            TryJump(isGrounded);
        }
       
        if (!isGrounded)
        {
            rb.linearVelocityY -= grav; 
        }
        else
        {
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
       

        else if (!onGround && hit && rb.linearVelocityY < 0)
        {
            queueJump = true;
            Debug.Log("QUEUED");
        }

        else if (canCoyote && rb.linearVelocityY < 0)
        {
            Debug.Log("YOTED");
            canCoyote = false;
            UseJump();
        }
    }

    private void UseJump()
    {
        rb.linearVelocityY = jumpSpeed;
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


            if (Mathf.Abs(rb.linearVelocityX) < 0.1f)
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
        RaycastHit2D hit = Physics2D.BoxCast(rb.position, Vector2.one, 0f, Vector2.down, 0.1f, groundLayer);

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
}
