using UnityEngine;

public class Player_Walk : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey;
    private bool _rev;
    private bool _x_axis_blocked;
    public Player_Walk(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
    }

    public override void EnterState() {
         //Debug.Log("Enter Walk state");

        Context._movementState = "Walking";
        _rev = Context._player.rev;
        _x_axis_blocked = Context._player.x_axis_blocked;
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Walk;
    }
    public override void UpdateState() {
        float moveInput = Input.GetAxisRaw("Horizontal");
        int direction = 0;
        _x_axis_blocked = Context._player.x_axis_blocked;
        //Debug.Log(!(moveInput < 0 && !_rev && _x_axis_blocked));
        if (moveInput > 0) { direction = 1; }
        else if (moveInput < 0) { direction = -1; }
        if (!(moveInput > 0 && _rev && _x_axis_blocked) && !(moveInput < 0 && !_rev && _x_axis_blocked))
        {
            Context.customRb.velocity.x = direction * Context._moveSpeed;
        }
        else
        {
            Context.customRb.velocity.x = 0f;
        }
        
        if(moveInput==0f){
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }
        if (Context.jumpRequest)
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
