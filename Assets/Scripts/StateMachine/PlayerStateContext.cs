using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContext
{
     // Define variable player parameters (debugging)
    public float _moveSpeed;
    public float _jumpForce;
    public float _lowJumpMultiplier;
    public float _fallMultiplier;
    public float _angledJump;
    
    // Set the LayerMask of the ground layer
    public LayerMask groundLayer;

    // Initialize custom Rigidbody class member for the player
    public CustomRigidbody2D customRb;
    private float _width;
    private float _height;

    // Ground check
    public bool isGrounded;

    // ducking check

    // groundCheck circle 
    public Transform groundCheck;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    private bool jumpRequest = false;

    public PlayerStateContext(float width,float height,float moveSpeed,float jumpForce,float lowJumpMultiplier, float fallMultiplier, float angledJump, Vector3 position )
    {
        customRb = new CustomRigidbody2D(width,height);
        customRb.position = position;
        _width = width;
        _height = height;
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _lowJumpMultiplier = lowJumpMultiplier;
        _fallMultiplier = fallMultiplier;
        _angledJump = angledJump;
    }

    public CustomRigidbody2D Rigidbody => customRb;
    public float Width => _width;
    public float Height => _height;

}
