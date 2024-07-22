using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContext
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
    private float _width;
    private float _height;

    // Ground check
    public bool isGrounded;

    // ducking check
    
    private bool isDucked=false;

    // groundCheck circle 
    private Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    private bool jumpRequest = false;

    public PlayerStateContext(CustomRigidbody2D rb,float width,float height)
    {
        rigidbody = rb;
        _width = width;
        _height = height;
    }

    public CustomRigidbody2D Rigidbody => rigidbody;
    public float Width => _width;
    public float Height => _height;

}
