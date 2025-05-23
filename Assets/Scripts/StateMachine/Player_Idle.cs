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
        Context._player.isBlocking = false;
        Context._isCrouching = false;
        Context.animator.SetInteger("State", 0);
        Context.animator.SetInteger("Form", 0);
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
        float inHorz = Input.GetAxis(Context._player._MH_in);
        float inVert = Input.GetAxis(Context._player._MV_in);
        if (Context.isAttacking)
        {
            Context.isAttacking = false;
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
            
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

        if (Context._isHit)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Hit;
            Context._isHit = false;
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
