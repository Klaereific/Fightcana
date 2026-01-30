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
        //Debug.Log("Enter Idle state");
        Context.customRb.velocity.x = 0f;
        Context._movementState = "Idle";
        Context._player.isBlocking = false;
        Context._isCrouching = false;
        Context.animator.SetInteger("State", 0);
        Context.animator.SetInteger("Form", 0);
    }
    public override void ExitState() {
        if (Context._isHit)
        {
            Context._isHit = false;    
        }
        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }
    public override void UpdateState() {

        byte[] currentFrame = Context._buffer.GetCurrentFrame();
        byte press = currentFrame[0];
        byte hold = currentFrame[1];

        byte input = (byte)(press | hold); 

        bool isUp    = (input & 0b00000100) != 0; // Bit 2
        bool isDown  = (input & 0b00000001) != 0; // Bit 0
        bool isRight = (input & 0b00000010) != 0; // Bit 1
        bool isLeft  = (input & 0b00001000) != 0; // Bit 3

        if (Context.isAttacking) {
            Context.isAttacking = false;
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
        }
        else if (isUp) {
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
        }
        else if (isDown) {
            nextStateKey = PlayerStateMachine.EPlayerState.Duck;
        }
        else if (isLeft || isRight) {
            nextStateKey = PlayerStateMachine.EPlayerState.Walk;
        }
        if (Context._isHit)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Hit;
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
