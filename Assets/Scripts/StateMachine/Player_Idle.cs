using UnityEngine;

public class Player_Idle : PlayerState
{
    PlayerStateMachine.EPlayerState nextStateKey; 
    public Player_Idle(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        PlayerStateContext Context = context;
        nextStateKey = StateKey;
    }

    public override void EnterState() {
        Debug.Log("Enter Idle state");
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }
    public override void UpdateState() {
        
        float moveInput = Input.GetAxisRaw("Horizontal");
        if(moveInput!=0f){
            
            nextStateKey = PlayerStateMachine.EPlayerState.Walk;
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
