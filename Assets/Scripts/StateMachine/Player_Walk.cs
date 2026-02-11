using UnityEngine;

public class Player_Walk : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey;
    private bool _rev;
    private bool _x_axis_blocked;
    private float _moveSpeed;
    private float _moveSpeed_back;
    //private float _moveInput;
    private int _direction;
    public Player_Walk(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
    }

    public override void EnterState() 
    {
        Context._movementState = "Walking";
        _rev = Context._player.rev;
        _moveSpeed = Context._moveSpeed;
        _moveSpeed_back = _moveSpeed * 0.7f;
        _x_axis_blocked = Context._player.x_axis_blocked;
        Context.animator.SetInteger("State", 1);
    }

    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Walk;
    }
    //public override void UpdateState() {
//
    //    _x_axis_blocked = Context._player.x_axis_blocked;
    //    _moveInput = Input.GetAxisRaw(Context._player._MH_in);
    //    if (Context._isBlocking)
    //    {
    //        nextStateKey = PlayerStateMachine.EPlayerState.Blocking;
    //    }
    //    else if (Context._isHit)
    //    {
    //        nextStateKey = PlayerStateMachine.EPlayerState.Hit;
    //        Context._isHit = false;
    //    }
    //    else if (Context.isAttacking)
    //    {
    //        Context.isAttacking = false;
    //        nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
//
    //    }else if (!(_moveInput > 0 && _rev && _x_axis_blocked) && !(_moveInput < 0 && !_rev && _x_axis_blocked))
    //    {
    //        if (Context._player.isBlocking)
    //        {
    //            Context.customRb.velocity.x = _direction * _moveSpeed_back;
    //        }
    //        else
    //        {
    //            Context.customRb.velocity.x = _direction * _moveSpeed;
    //        }
    //    }
    //    else
    //    {
    //        Context.customRb.velocity.x = 0f;
    //    }
//
    //    if (_moveInput == 0f){
    //        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    //    }
    //    if  (Input.GetAxis(Context._player._MV_in) > 0.5)
    //    {
    //        nextStateKey = PlayerStateMachine.EPlayerState.Jump;
    //    }
    //    if (Input.GetButtonDown("Duck"))
    //    {
    //        nextStateKey = PlayerStateMachine.EPlayerState.Duck;
    //    }
    //    
    //}

    public override void UpdateState() {
        // Read current frame input from the buffer
        byte[] currentFrame = Context._buffer.GetCurrentFrame();
        byte input = (byte)(currentFrame[0] | currentFrame[1]); 

        bool isRight = (input & 0b00000010) != 0;
        bool isLeft  = (input & 0b00001000) != 0;

        // Determine movement direction
        _direction = 0;
        if (isRight) _direction = 1;
        else if (isLeft) _direction = -1;

        // Transition back to Idle if no direction is held
        if (_direction == 0) {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            Context.customRb.velocity.x = 0;
            return;
        }

        // Check if movement is blocked by a wall or opponent collision
        _x_axis_blocked = Context._player.x_axis_blocked;
        if (!(_direction > 0 && !_rev && _x_axis_blocked) && !(_direction < 0 && _rev && _x_axis_blocked))
        {
            // Apply speed (slower if blocking)
            float speed = Context._player.isBlocking ? _moveSpeed_back : _moveSpeed;
            Context.customRb.velocity.x = _direction * speed;
        }
        else
        {
            Context.customRb.velocity.x = 0f;
        }

        // 5. Check for State Transitions
        if (Context._isHit)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Hit;
        }
        else if ((input & 0b00000100) != 0) // Bit 2 = Up (Jump)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        else if ((input & 0b00000001) != 0) // Bit 0 = Down (Duck)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Duck;
        }
        else if (input > 15) // Any bits 4-7 active = Attack Button
        {
            Context.isAttacking = true;
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
        }
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return nextStateKey;
    }
    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}
}
