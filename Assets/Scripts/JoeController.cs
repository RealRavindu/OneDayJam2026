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
    public Vector2 goalVelo;


    float hInput; 
    [Header("Jump Variables")]    
    public float apexTime;
    public float apexHeight;
    public float initialJumpVelo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        hAccel = hMaxSpeed / hAccelTime;

      
    }

    private void FixedUpdate()
    {
        if (playerInput.x != 0)
        {
            rb.linearVelocityX += hAccel * playerInput.x * Time.deltaTime;
        }

        else
        {
            float frictionDirection = -Mathf.Sign(rb.linearVelocityX);
            Debug.Log(frictionDirection);
        }
    }
}
