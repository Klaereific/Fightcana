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
        Attacking
    }

    public enum Buttons
    {
        light_attack,
        medium_attack,
        heavy_attack,
        special_attack
    }
    public enum MovementButtons
    {
        Forward,
        Forward_Down,
        Down,
        Backward_Down,
        Backward,
        Backward_Up,
        Up,
        Forward_Up
    }

    // [SerializeField] private CustomRigidbody2D customRb;
    [SerializeField] private float width;
    [SerializeField] private float height;
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 3.0f;
    public float angledJump = 3.0f;

    public GameObject hitboxPrefab;
    
    private PlayerStateContext _context;

    public PlayerControls _controls;

    public GameObject playerGO;
    public Player player;
    
    private InputBuffer inputBufferP1;
    // Start is called before the first frame update
    void Awake()
    {
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();

        /*{
        inputBufferP1 = playerGO.GetComponent<InputBuffer>();

        inputBufferP1.InitializeBuffer(40, player);
        
        inputBufferP1.StartBuffer();
        }*/

        width = playerGO.transform.localScale.x;
        height = playerGO.transform.localScale.y;
        _context = new PlayerStateContext(playerGO, moveSpeed, jumpForce, lowJumpMultiplier,fallMultiplier,angledJump,hitboxPrefab);
        _context.groundCheck = transform.Find("GroundCheck");
        
        //_controls = new PlayerControls();

        //InititalizeControls();
        //_controls.Enable();

        InitializeStates();
        currentState.EnterState();

    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _context.customRb.UpdatePhysics(Time.fixedDeltaTime);
        playerGO.transform.position = _context.customRb.position;
        currentState.UpdateState();
        //Debug.Log(Input.GetAxis("MoveVertical"));

    }
    public void Update()
    {
        UpdateInput();
        DrawCube(transform.position,_context.customRb.size,Color.green);
        DrawCube(_context.customRb.position, _context.customRb.size, Color.red);

    }

    

    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new Player_Idle(_context,EPlayerState.Idle));
        States.Add(EPlayerState.Walk, new Player_Walk(_context,EPlayerState.Walk));
        States.Add(EPlayerState.Duck, new Player_Duck(_context,EPlayerState.Duck));
        States.Add(EPlayerState.Jump, new Player_Jump(_context,EPlayerState.Jump));
        States.Add(EPlayerState.Attacking, new Player_Attacking(_context, EPlayerState.Attacking));
        currentState = States[EPlayerState.Idle];
    }
    /*{
    private void InititalizeControls()
    {
        _controls.Gameplay.LightAttack.performed += ctx => _context.button_queue.Enqueue(Buttons.light_attack);
    }
    }*/

    private void UpdateInput()
    {
        
        if (Input.GetAxis("MoveVertical") < -0.5f)
        {
            _context.jumpRequest = true;
        }
        /*
        if (Input.GetButton("X"))
        {
            Debug.Log("Light attack pressed");
            _context.button_queue.Enqueue(Buttons.light_attack);
        }
        if (Input.GetButton("Y"))
        {
            _context.button_queue.Enqueue(Buttons.medium_attack);
        }
        if (Input.GetButton("B"))
        {
            _context.button_queue.Enqueue(Buttons.heavy_attack);
        }
        if (Input.GetButton("A"))
        {
            _context.button_queue.Enqueue(Buttons.special_attack);
        }
        if (Input.GetButton("R1"))
        {
            _context.button_queue.Enqueue(Buttons.dash);
        }
        */
    }

    public byte GetInput()
    {

        int input = 0;
        if (Input.GetButton("X")) //West
        {
            input += 2 ^ 7;
        }
        if (Input.GetButton("Y")) //North
        {
            input += 2 ^ 6;
        }
        if (Input.GetButton("B")) //East
        {
            input += 2 ^ 5;
        }
        if (Input.GetButton("A")) //South
        {
            input += 2 ^ 4;
        }
        if (Input.GetAxis("MoveHorizontal") < -0.5f) //Move left
        {
            input += 2 ^ 3;
        }
        if (Input.GetAxis("MoveVertical") > 0.5f) //Move up
        {
            input += 2 ^ 2;
        }
        if (Input.GetAxis("MoveHorizontal") > 0.5f) //Move right
        {
            input += 2 ^ 1;
        }
        if (Input.GetAxis("MoveVertical") < -0.5f) //Move down
        {
            input += 2 ^ 0;
        }
        Debug.Log(input);
        return (byte)input;

    }

    public static void SpawnHitbox(GameObject hitboxPrefab,Player player,Vector3 position, Quaternion rotation, Vector2 size, float damage, float duration, Color color)
    {
        //Debug.Log("Hitbox spawned");
        GameObject hitbox = Instantiate(hitboxPrefab, position, rotation);
        hitbox.transform.localScale = size;
        hitbox.GetComponent<SpriteRenderer>().color = color;
        hitbox.GetComponent<Hitbox>().damage = damage;  // Set the damage value if needed
        hitbox.GetComponent<Hitbox>().sourcePlayer = player;
        Destroy(hitbox, duration);  // Destroy the hitbox after 0.5 seconds to simulate attack duration
    }

    private void LightAttack()
    {
        //Debug.Log("Light attack pressed");
    }
    
    public void DrawCube(Vector2 position, Vector2 size,Color color)
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
}
