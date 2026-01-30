using UnityEngine;

public class Player_Duck : PlayerState
{
    public Player_Duck(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        // PlayerStateContext Context = context;
    }

    public override void EnterState() {
        //Debug.Log("Enter Ducked state");
        Context.customRb.velocity.x = 0f;
        Context._movementState = "Crouching";
        Context.animator.SetInteger("State", 3);
        if (!Context._isCrouching)
        {
            ApplyDuckTransform();
        }
        
        
    }
    public override void ExitState() {
        ApplyStandTransform();
        nextStateKey = PlayerStateMachine.EPlayerState.Duck;
    }
    public override void UpdateState() {
        // Read Press (0) and Hold (1) from buffer
        byte[] currentFrame = Context._buffer.GetCurrentFrame();
        byte press = currentFrame[0];
        byte hold = currentFrame[1];
        byte input = (byte)(press | hold);

        bool isDownHeld = (input & 0b00000001) != 0; // Bit 0

        // 1. Check if we should stand up
        if (!isDownHeld)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            return;
        }

        // 2. Handle Blocking while crouching
        // If holding away from opponent (Bit 1 is Right, Bit 3 is Left)
        bool isRight = (input & 0b00000010) != 0;
        bool isLeft  = (input & 0b00001000) != 0;
        
        // Simple logic: if any horizontal input is held while ducking, enable block
        Context._player.isBlocking = (isLeft || isRight);

        // 3. Attack check
        if (input > 15)
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
    private void ApplyDuckTransform()
    {
        // Instead of moving the Y position (which causes sinking), 
        // we only adjust the scale of the collision box.
        float duckedHeight = Context._height / 2;
        Context.customRb.SetScale(Context._width, duckedHeight);
        Context._isCrouching = true;
    }

    private void ApplyStandTransform()
    {
        Context.customRb.SetScale(Context._width, Context._height);
        Context._isCrouching = false;
    }

}
