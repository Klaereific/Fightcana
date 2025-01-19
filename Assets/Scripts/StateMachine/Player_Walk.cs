using UnityEngine;

public class Player_Walk : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey;
    private bool _rev;
    private bool _x_axis_blocked;
    private float _moveSpeed;
    private float _moveSpeed_back;
    private float _moveInput;
    private int _direction;
    public Player_Walk(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
    }

    public override void EnterState() {
         //Debug.Log("Enter Walk state");

        Context._movementState = "Walking";
        _rev = Context._player.rev;
        
        _moveSpeed = Context._moveSpeed;
        _moveSpeed_back = _moveSpeed * 0.7f;

        _moveInput = Input.GetAxisRaw(Context._player._MH_in);
        _direction = 0;
        _x_axis_blocked = Context._player.x_axis_blocked;
        //Debug.Log(!(moveInput < 0 && !_rev && _x_axis_blocked));
        Context.animator.SetInteger("State", 1);
        if (_moveInput > 0) { _direction = 1; }
        else if (_moveInput < 0) {_direction = -1; }
        if ((_direction < 0 && !_rev) || (_direction > 0 && _rev))
        {
            Context.animator.SetInteger("Form", 1);
            Context._player.isBlocking = true;
        }
        else
        {
            Context.animator.SetInteger("Form", 0);
        }

    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Walk;
    }
    public override void UpdateState() {

        _x_axis_blocked = Context._player.x_axis_blocked;
        _moveInput = Input.GetAxisRaw(Context._player._MH_in);
        if (Context._isBlocking)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Blocking;
        }
        if (Context.isAttacking)
        {
            Context.isAttacking = false;
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;

        }else if (!(_moveInput > 0 && _rev && _x_axis_blocked) && !(_moveInput < 0 && !_rev && _x_axis_blocked))
        {
            if (Context._player.isBlocking)
            {
                Context.customRb.velocity.x = _direction * _moveSpeed_back;
            }
            else
            {
                Context.customRb.velocity.x = _direction * _moveSpeed;
            }
        }
        else
        {
            Context.customRb.velocity.x = 0f;
        }

        if (_moveInput == 0f){
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }
        if  (Input.GetAxis(Context._player._MV_in) > 0.5)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        if (Input.GetButtonDown("Duck"))
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Duck;
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
