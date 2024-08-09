using UnityEngine;

public class Player_Idle : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey; 
    public Player_Idle(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
    }

    public override void EnterState() {
        Debug.Log("Enter Idle state");
        Context.customRb.velocity.x = 0f;
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }
    public override void UpdateState() {
        
        float moveInput = Input.GetAxis("MoveHorizontal");
        if(moveInput!=0f){
            
            nextStateKey = PlayerStateMachine.EPlayerState.Walk;
        }
        if (Context.jumpRequest)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        if (Input.GetAxis("MoveVertical") > 0.5f)
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
