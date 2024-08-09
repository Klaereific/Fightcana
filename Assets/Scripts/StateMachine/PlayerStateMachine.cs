using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState{
        Idle,
        Walk,
        Duck,
        Jump
    }

    // [SerializeField] private CustomRigidbody2D customRb;
    [SerializeField] private float width;
    [SerializeField] private float height;
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 3.0f;
    public float angledJump = 3.0f;
    
    
    private PlayerStateContext _context;
    
    // Start is called before the first frame update
    void Awake()
    {
        width = transform.localScale.x;
        height = transform.localScale.y;
        _context = new PlayerStateContext(width,height,moveSpeed,jumpForce,lowJumpMultiplier,fallMultiplier,angledJump,transform.position,this.transform);
        _context.groundCheck = transform.Find("GroundCheck");
        

        InitializeStates();
        currentState.EnterState();

    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _context.customRb.UpdatePhysics(Time.fixedDeltaTime);
        transform.position = _context.customRb.position;
        currentState.UpdateState();
        Debug.Log(Input.GetAxis("MoveVertical"));
        
    }
    public void Update()
    {
        
        if (Input.GetAxis("MoveVertical") < -0.5f)
        {
            _context.jumpRequest = true;
        }
    }

    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new Player_Idle(_context,EPlayerState.Idle));
        States.Add(EPlayerState.Walk, new Player_Walk(_context,EPlayerState.Walk));
        States.Add(EPlayerState.Duck, new Player_Duck(_context,EPlayerState.Duck));
        States.Add(EPlayerState.Jump, new Player_Jump(_context,EPlayerState.Jump));
        currentState = States[EPlayerState.Idle];
    }
}
