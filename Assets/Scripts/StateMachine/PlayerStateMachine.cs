using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState{
        Idle,
        Walk,
        Duck,
        Jump,
        Attacking,
        Blocking,
        Hit,
        Knocked
    }

    public enum Buttons
    {
        light_attack,
        medium_attack,
        heavy_attack,
        special_attack
    }
    //public enum MovementButtons
    //{
    //    Forward,
    //    Forward_Down,
    //    Down,
    //    Backward_Down,
    //    Backward,
    //    Backward_Up,
    //    Up,
    //    Forward_Up
    //}

    // [SerializeField] private CustomRigidbody2D customRb;
    [SerializeField] private float width;
    [SerializeField] private float height;
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 3.0f;
    public float angledJump = 3.0f;

    public bool flipped;

    public GameObject hitboxPrefab;
    
    protected PlayerStateContext _context;

    protected PlayerControls _controls;

    public GameObject playerGO;
    public Player player;

    public Animator animator;

    public float rb_margin=0.02f;

    protected Vector2 _rawMoveInput;

    public DeterministicPhysics customRB;

    public InputBuffer _inputBuffer;

    public Transform opponentTransform;

    private void UpdateFacingDirection()
{
    if (opponentTransform != null)
    {
        bool shouldBeFlipped = transform.position.x > opponentTransform.position.x;
        
        if (_context._player.rev != shouldBeFlipped)
        {
            _context._player.rev = shouldBeFlipped;

            // CORRECT: We keep Y and Z at 1, only flipping X
            // This is like looking at the sprite in a mirror rather than turning it
            float xMag = 2.0f;
            transform.localScale = new Vector3(shouldBeFlipped ? -xMag : xMag, 0.500007f, 0.9999714f);

            // FORCE ROTATION TO ZERO: This prevents the "Sheet of Paper" rotation
            transform.rotation = Quaternion.identity;
        }
    }
}

    // Start is called before the first frame update
    //protected void Awake()
    //{
    //    
    //    playerGO = transform.parent.gameObject; 
    //    player = playerGO.GetComponent<Player>();
    //
    //    string[] controllers = Input.GetJoystickNames();
    //    for(int i=0;i<controllers.Length; i++){
    //        Debug.Log(i+": "+controllers[i]);
    //    }
    //    
    //    /*{
    //    inputBufferP1 = playerGO.GetComponent<InputBuffer>();
    //
    //    inputBufferP1.InitializeBuffer(40, player);
    //    
    //    inputBufferP1.StartBuffer();
    //    }*/
    //
    //    width = playerGO.transform.localScale.x;
    //    height = playerGO.transform.localScale.y;
    //    flipped = false;
    //    _context = new PlayerStateContext(playerGO, moveSpeed, jumpForce, lowJumpMultiplier,fallMultiplier,angledJump,hitboxPrefab, rb_margin);
    //    _context.groundCheck = transform.Find("GroundCheck");
    //    
    //    //_controls = new PlayerControls();
    //
    //    //InititalizeControls();
    //    //_controls.Enable();
    //
    //    InitializeStates();
    //    currentState.EnterState();
    //
    //}

    protected void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Move.performed += ctx => _rawMoveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += ctx => _rawMoveInput = Vector2.zero;

        _context = new PlayerStateContext(playerGO, this, moveSpeed, jumpForce, lowJumpMultiplier, fallMultiplier, angledJump, hitboxPrefab, rb_margin);
        _context.animator = animator;
        
        _context.StartInputBuffer();

        InitializeStates();

        currentState = States[EPlayerState.Idle];
    }

    // Update is called once per frame
    //public override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    _context.customRb.UpdatePhysics(Time.fixedDeltaTime);
    //    Vector2 adj_pos = _context.customRb.position;
    //    adj_pos.y = _context.customRb.position.y - ((_context.customRb.size.y - 1) / 2);
    //    playerGO.transform.position = adj_pos;
    //    
    //    if (player.rev)
    //    {
    //        if (!flipped)
    //        {
    //            playerGO.GetComponent<SpriteRenderer>().flipX = true;
    //            flipped=true;
    //        }
    //    }
    //    else
    //    {
    //        if (flipped)
    //        {
    //            playerGO.GetComponent<SpriteRenderer>().flipX = false;
    //            flipped = false;
    //        }
    //    }
    //    //currentState.UpdateState();
    //    //Debug.Log(Input.GetAxis("MoveVertical"));
    //    }
    //public void Update()
    //{
    //    // Update ground checking
    //    _context.UpdateGroundCheck();
    //    
    //    //UpdateInput();
    //    //DrawBox(transform.position,_context.customRb.size,Color.green);
    //    DrawBox(_context.customRb.position, _context.customRb.size, Color.red);
    //    //Debug.Log(_context._player.isBlocking);
    //}

    private void Update()
    {
        if (_context._buffer != null)
        {
            _context._buffer.UpdateRawInput(_rawMoveInput);
        }
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new Player_Idle(_context,EPlayerState.Idle));
        States.Add(EPlayerState.Walk, new Player_Walk(_context,EPlayerState.Walk));
        States.Add(EPlayerState.Duck, new Player_Duck(_context,EPlayerState.Duck));
        States.Add(EPlayerState.Jump, new Player_Jump(_context,EPlayerState.Jump));
        States.Add(EPlayerState.Attacking, new Player_Attacking(_context, EPlayerState.Attacking));
        States.Add(EPlayerState.Blocking, new Player_Blocking(_context, EPlayerState.Blocking));
        States.Add(EPlayerState.Hit, new Player_Hit(_context, EPlayerState.Hit));
        States.Add(EPlayerState.Knocked, new Player_Knocked(_context, EPlayerState.Knocked));
        currentState = States[EPlayerState.Idle];
    }
    /*{
    private void InititalizeControls()
    {
        _controls.Gameplay.LightAttack.performed += ctx => _context.button_queue.Enqueue(Buttons.light_attack);
    }
    }*/


    public static void SpawnHitbox(GameObject hitboxPrefab,Player player,Vector3 position, Quaternion rotation, Vector2 size, float damage, int blockstun, int hitstun, float blockForce, float hitForce,  float duration, Color color)
    {
        // Debug.Log(duration*60);
        GameObject hitbox = Instantiate(hitboxPrefab, position, rotation);
        
        hitbox.transform.localScale = size;
        Color adj_color = color;
        adj_color.a = 0.2f;
        hitbox.GetComponent<SpriteRenderer>().color = adj_color;

        //hitbox.GetComponent<Hitbox>().damage = damage;  // Set the damage value if needed
        //hitbox.GetComponent<Hitbox>().sourcePlayer = player;

        Hitbox hitboxComponent = hitbox.GetComponent<Hitbox>();

        //hitbox.GetComponent<Hitbox>().hitstun = hitstun;
        //hitbox.GetComponent<Hitbox>().blockstun = blockstun;
        //hitbox.GetComponent<Hitbox>().blockForce = blockForce;
        //hitbox.GetComponent<Hitbox>().hitForce = hitForce;
    
        hitboxComponent.sourceTransform = player.transform; 

        hitboxComponent.damage = damage; 
        hitboxComponent.hitstun = hitstun;
        hitboxComponent.blockstun = blockstun;
        hitboxComponent.blockForce = blockForce;
        hitboxComponent.hitForce = hitForce;

        Destroy(hitbox, duration);
    }

    private void LightAttack()
    {
        //Debug.Log("Light attack pressed");
    }
    
    public void DrawBox(Vector2 position, Vector2 size,Color color)
    {
        Vector2 br = new Vector2(position.x + size.x / 2, position.y - size.y / 2);
        Vector2 bl = new Vector2(position.x - size.x / 2, position.y - size.y / 2);
        Vector2 tr = new Vector2(position.x + size.x / 2, position.y + size.y / 2);
        Vector2 tl = new Vector2(position.x - size.x / 2, position.y + size.y / 2);
        Debug.DrawLine(bl, br, color,0.01f); // bottom left to bottom right
        Debug.DrawLine(bl, tl, color, 0.01f);
        Debug.DrawLine(br, tr, color, 0.01f);
        Debug.DrawLine(tl, tr, color, 0.01f);
    }

    public override void FixedUpdate()
    {
        UpdateFacingDirection();

        base.FixedUpdate();

        float TICK_RATE = 1f / 60f; 
        _context.customRb.UpdatePhysics(TICK_RATE);
        // TEMPORARY TEST: Manually move the parent based on the context velocity
        if (_context.customRb.position.y < -0.5f)
        {
            //_context.customRb.UpdatePhysics(Time.fixedDeltaTime);
            //Vector3 movement = new Vector3(_context.customRb.velocity.x, _context.customRb.velocity.y, 0);
            //playerGO.transform.position += movement * Time.fixedDeltaTime;
            _context.customRb.position.y = -0.5f;
            _context.customRb.velocity.y = 0;
        }
        playerGO.transform.position = new Vector3(_context.customRb.position.x, _context.customRb.position.y, 0);
    }
}
