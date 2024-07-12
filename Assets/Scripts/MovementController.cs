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
    private float width;
    private float height;

    // Ground check
    public bool isGrounded;

    // ducking check
    
    private bool isDucked;

    // groundCheck circle 
    private Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    private bool jumpRequest;

    public string state;

    private void Start()
    {

        width = transform.localScale.x;
        height = transform.localScale.y;

        // Initialize custom rigidbody
        customRb = new CustomRigidbody2D(width,height);
        customRb.position = transform.position;

        state = "init";

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
        if(isGrounded && !isDucked)
        {
            customRb.velocity.x = moveInput * moveSpeed;
        }

        // Handle jump
        if (!isDucked && jumpRequest)
        {
            //customRb.ApplyForce(new Vector2(0,jumpForce));
            Jump(moveInput);
            /*
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
            */

        }
        if(!isDucked && Input.GetButton("Duck")&& isGrounded)
        {
            customRb.velocity.x = 0;
            Duck();
        }
        if(isDucked && !Input.GetButton("Duck"))
        {
            Stand();
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
        // Debug.Log(customRb.velocity.y);

        // Update custom physics
        customRb.UpdatePhysics(Time.fixedDeltaTime);

        // Apply position to transform
        transform.position = customRb.position;

        
        if(Math.Abs(customRb.velocity.x) < 0.01f)
        {
            if (customRb.velocity.y < 0)
            {
                state="stationary fall";
            }
            else if(customRb.velocity.y > 0)
            {
                state="stationary jump";
            }
            else if(isDucked)
            {
                state="ducked";
            }
            else
            {
                state="standing";
            }
        }
        else if(moveInput < 0)
        {
            if (customRb.velocity.y < 0)
            {
                state="backwards fall";
            }
            else if(customRb.velocity.y > 0)
            {
                state="backwards jump";
            }
            else
            {
                state="back walk";
            }   
        }else
        {
            if (customRb.velocity.y < 0)
            {
                state="forward fall";
            }
            else if(customRb.velocity.y > 0)
            {
                state="forward jump";
            }
            else
            {
                state="forward walk";
            }
        }

        //Debug.Log(state);
    }


    public void Duck()
    {
        float duckedheight = height / 2;
        customRb.position = new Vector2(transform.position.x,transform.position.y-duckedheight/2);
        customRb.SetScale(width,duckedheight);
        transform.localScale = new Vector2(width,duckedheight);
        isDucked = true;
    }

    public void Stand()
    {   
        customRb.position = transform.position;
        customRb.SetScale(width,height);
        transform.localScale = new Vector2(width,height);
        isDucked = false;
    }

    public void Jump(float moveInput)
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

}