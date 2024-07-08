using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Define variable player parameters (debugging)
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 3.0f;
    public float angledJump = 3.0f;

    // Set the LayerMask of the ground layer
    public LayerMask groundLayer;

    // Initialize custom Rigidbody class member for the player
    private CustomRigidbody2D customRb;
    // Ground check
    public bool isGrounded;
    // groundCheck circle 
    private Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    private bool jumpRequest;


    private void Start()
    {
        // Initialize custom rigidbody
        customRb = new CustomRigidbody2D(transform.localScale.x,transform.localScale.y);
        customRb.position = transform.position;

        // Find the ground check object
        groundCheck = transform.Find("GroundCheck");
        if(groundCheck == null)
        {
            Debug.LogError("GroundCheck object not found. Please create an empty GameObject named 'GroundCheck' as a child of the player.");
        }

    }

    private void Update()
    {
        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }

    }

    private void FixedUpdate()
    {
        // Ground check
        // function takes position, radius, layer filter and returns the Collider or int 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Input handling
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Move the player
        if(isGrounded)
        {
            customRb.velocity.x = moveInput * moveSpeed;
        }

        // Handle jump
        if (jumpRequest)
        {
            //customRb.ApplyForce(new Vector2(0,jumpForce));
            if(Math.Abs(moveInput)<0.01f)
            {
                customRb.velocity.y = jumpForce;
                jumpRequest = false;
            }else
            {
                customRb.velocity.y = jumpForce;
                customRb.velocity.x = moveInput * angledJump;
                jumpRequest = false;
            }

        }

        // Apply better jump mechanics
        if (customRb.velocity.y < 0)
        {
            customRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; 
        }
        else if (customRb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            customRb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }else if(!isGrounded)
        {
            customRb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        }
        Debug.Log(customRb.velocity.y);
        // Update custom physics
        customRb.UpdatePhysics(Time.fixedDeltaTime);

        // Apply position to transform
        transform.position = customRb.position;
    }
}