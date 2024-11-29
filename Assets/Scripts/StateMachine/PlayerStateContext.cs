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

    // public TimedQueue<PlayerStateMachine.Buttons> button_queue;
    // public TimedQueue<PlayerStateMachine.MovementButtons> movement_queue;
    public Vector2 vertMovement;
    public Vector2 horzMovement;

    public string _movementState;

    // Ground check
    public bool isGrounded;

    public bool isAttacking;
    // ducking check

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

    private float _rb_margin;

    public bool _isHit=false;
    public Animator animator;

    public PlayerStateContext(GameObject playerGO,float moveSpeed,float jumpForce,float lowJumpMultiplier, float fallMultiplier, float angledJump,GameObject hitboxPref,float rb_margins)
    {
        playerTransform = playerGO.GetComponent<Transform>();
        //playerTransform = playertransform;
        _width = playerTransform.localScale.x;
        _height = playerTransform.localScale.y;
        customRb = new CustomRigidbody2D(_width, _height,playerGO, rb_margins);
        customRb.position = playerTransform.position;

        _player = playerGO.GetComponent<Player>();
        //_player = player;
        _movementState = "Idle";
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _lowJumpMultiplier = lowJumpMultiplier;
        _fallMultiplier = fallMultiplier;
        _angledJump = angledJump;
        _p1_CP = playerGO.GetComponent<CharacterParameters>();
        _hitboxPrefab = hitboxPref;

        _buffer = playerGO.GetComponent<InputBuffer>();

        _buffer.InitializeBuffer(40, _player);
        _buffer.StartBuffer();

        _buffer.OnButtonInput += OnButtonInput;

        _hitbox = hitboxPref.GetComponent<Hitbox>();
        _player.OnHit += OnHit;

        _player.OnBlock += OnBlock;

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

    public void OnBlock(object source, int Block_Stun)
    {
        _blockStun = Block_Stun;
    }

    public void OnHit(object source, int Hit_Stun)
    {
        _hitStun = Hit_Stun;
        _isHit = true;
    }
}
