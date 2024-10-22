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
        Context._movementState = "Idle";
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }
    public override void UpdateState() {

        /* // Byte implementation (inefficient)
        
        byte input_byte = Context._buffer.GetCurrentFrame()[0];
        if ((int)input_byte > 15)  // "Attack" button pressed
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
        }
        if ((input_byte & 0b00000100) != 0)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        if ((input_byte & 0b00000001) != 0)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Duck;
        }
        if (((input_byte & 0b00000010) !=0)|((input_byte & 0b00001000)!=0)){
            
            nextStateKey = PlayerStateMachine.EPlayerState.Walk;
        }
        */
        float inHorz = Input.GetAxis("MoveHorizontal");
        float inVert = Input.GetAxis("MoveVertical");
        if (Context.isAttacking)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
            Context.isAttacking = false;
        }
        if (inVert > 0.5f)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        else if (inVert < -0.5f)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Duck;
        }
        else if (inHorz != 0)
        {

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
