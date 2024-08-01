using UnityEngine;

public class Player_Walk : PlayerState
{
    PlayerStateMachine.EPlayerState nextStateKey;
    public Player_Walk(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        PlayerStateContext Context = context;
        nextStateKey = StateKey;
    }

    public override void EnterState() {
         Debug.Log("Enter Walk state");
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Walk;
    }
    public override void UpdateState() {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Context.customRb.velocity.x = moveInput * Context._moveSpeed;
        Debug.Log("MoveSpeed");
        Debug.Log(Context._moveSpeed);
        Debug.Log("Velocity");
        Debug.Log(Context.customRb.velocity.x);
        if(moveInput==0f){
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
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
