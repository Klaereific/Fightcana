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
    // private GameObject player;


    public CustomRigidbody2D customRb;
    public float _width;
    public float _height;

    public TimedQueue<PlayerStateMachine.Buttons> button_queue;
    public TimedQueue<PlayerStateMachine.MovementButtons> movement_queue;
    public Vector2 vertMovement;
    public Vector2 horzMovement;

    public string _movementState;

    // Ground check
    public bool isGrounded;

    // ducking check

    // groundCheck circle 
    public Transform groundCheck;

    public Transform playerTransform;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    public bool jumpRequest = false;

    public CharacterParameters _p1_CP;

    public GameObject _hitboxPrefab;

    public Player _player;

    public PlayerStateContext(Player player,float moveSpeed,float jumpForce,float lowJumpMultiplier, float fallMultiplier, float angledJump, Transform playertransform,GameObject hitboxPref)
    {
        playerTransform = playertransform;
        _width = playerTransform.localScale.x;
        _height = playerTransform.localScale.y;
        customRb = new CustomRigidbody2D(_width, _height);
        customRb.position = playerTransform.position;

        _player = player;
        _movementState = "Idle";
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _lowJumpMultiplier = lowJumpMultiplier;
        _fallMultiplier = fallMultiplier;
        _angledJump = angledJump;
        _p1_CP = new CharacterParameters(2f, _moveSpeed, new Vector2(_width, _height));
        _hitboxPrefab = hitboxPref;

        button_queue = new TimedQueue<PlayerStateMachine.Buttons>(10,60,60); // args: capacity, expiration time, exp time check timer
        movement_queue = new TimedQueue<PlayerStateMachine.MovementButtons>(10,60,60);
    }

    public CustomRigidbody2D Rigidbody => customRb;
    public float Width => _width;
    public float Height => _height;


}
