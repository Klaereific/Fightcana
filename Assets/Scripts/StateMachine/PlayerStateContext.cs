using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    // public TimedQueue<PlayerStateMachine.Buttons> button_queue;
    // public TimedQueue<PlayerStateMachine.MovementButtons> movement_queue;
    public Vector2 vertMovement;
    public Vector2 horzMovement;

    public string _movementState;

    // Ground check
    public bool isGrounded;

    public bool isAttacking;
    // ducking check
    public bool _isCrouching;

    // groundCheck circle 
    public Transform groundCheck;

    public Transform playerTransform;
    private float groundCheckRadius = 0.2f;
    
    // Jump request to queue input
    public bool jumpRequest = false;

    public CharacterParameters _p1_CP;

    public GameObject _hitboxPrefab;

    public Hitbox _hitbox;

    public Player _player;

    public InputBuffer _buffer;

    public byte[][] _buffer_state;
    
    public int _blockStun;
    public int _hitStun;
    public int _knockStun;

    public float _hitForce;
    public float _blockForce;

    private float _rb_margin;

    public bool _isHit=false;
    public bool _isBlocking = false;
    public Animator animator;

    public PlayerStateContext(GameObject playerGO,float moveSpeed,float jumpForce,float lowJumpMultiplier, float fallMultiplier, float angledJump,GameObject hitboxPref,float rb_margins)
    {
        playerTransform = playerGO.GetComponent<Transform>();
        _player = playerGO.GetComponent<Player>();


        _width = playerTransform.localScale.x * _player.width;
        _height = playerTransform.localScale.y * _player.height;
       

        customRb = new CustomRigidbody2D(_width, _height, playerTransform.localPosition.x, playerTransform.localPosition.y , playerGO, rb_margins);
        //customRb.position = playerTransform.position;

        //_player = playerGO.GetComponent<Player>();
        //_player = player;
        _movementState = "Idle";
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _lowJumpMultiplier = lowJumpMultiplier;
        _fallMultiplier = fallMultiplier;
        _angledJump = angledJump;
        
        _p1_CP = playerGO.GetComponent<CharacterParameters>();
        _hitboxPrefab = hitboxPref;
        _p1_CP.InitializeAttacks();

        _buffer = playerGO.GetComponent<InputBuffer>();

        _buffer.InitializeBuffer(40, _player);
        _buffer.StartBuffer();

        _buffer.OnButtonInput += OnButtonInput; 

        _hitbox = hitboxPref.GetComponent<Hitbox>();
        _player.OnHit += OnHit;                     // subscribes to event handler in player

        _player.OnBlock += OnBlock;                 // subscribes to event handler in player

        animator = playerGO.GetComponent<Animator>();

        


        //button_queue = new TimedQueue<PlayerStateMachine.Buttons>(10,60,60); // args: capacity, expiration time, exp time check timer
        //movement_queue = new TimedQueue<PlayerStateMachine.MovementButtons>(10,60,60);
    }

    public CustomRigidbody2D Rigidbody => customRb;
    public float Width => _width;
    public float Height => _height;

    public void OnButtonInput(object source, byte[][] buffer_state)
    {
        Debug.Log("Button pressed");
        isAttacking = true;
        _buffer_state = buffer_state;
    }

    public void OnBlock(object source, int Block_Stun, float blockForce)  // is called by player & updates values
    {
        _blockStun = Block_Stun;
        _isBlocking = true;
        _blockForce = blockForce;
    }

    public void OnHit(object source, int Hit_Stun, float hitForce)      // is called by player & updates values
    {
        _hitStun = Hit_Stun;
        _isHit = true;
        _hitForce = hitForce;
    }
}
