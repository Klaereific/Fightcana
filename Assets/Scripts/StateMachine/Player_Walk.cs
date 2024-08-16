using UnityEngine;

public class Player_Walk : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey;
    public Player_Walk(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
    }

    public override void EnterState() {
         Debug.Log("Enter Walk state");

        Context._movementState = "Walking";
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Walk;
    }
    public override void UpdateState() {
        float moveInput = Input.GetAxisRaw("Horizontal");
        int direction = 0;
        if(moveInput > 0) { direction = 1; }
        else if (moveInput < 0) { direction = -1; }
        Context.customRb.velocity.x = direction * Context._moveSpeed;
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
