using UnityEngine;

public class Player_Dashing : PlayerState
{
    private float dashDuration = 0.20f; 
    private float dashTimer;

    public Player_Dashing(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate) {}

    public override void EnterState() {
        Context._movementState = "Dashing";
        Context.animator.SetInteger("State", 4); 
        dashTimer = dashDuration;
        nextStateKey = StateKey;
        ApplyVelocity();
    }
    
    public override void UpdateState() {
        dashTimer -= Time.fixedDeltaTime;

        // 1. CANCEL LOGIC: Check for "Back"
        //byte[] currentFrame = Context._buffer.GetCurrentFrame();
        //byte combinedInput = currentFrame[0]; // Press or Hold
        //
        //// If facing right (flipped=false), "Back" is LEFT.
        //byte backMask = Context._player.rev ? InputButtons.RIGHT : InputButtons.LEFT;
        //
        //int backBit = Context._player.rev ? 1 : 3;
        //
        //if ((combinedInput & (1 << backBit)) != 0) 
        //{
        //    Debug.Log($"<color=red>Dash Cancelled!</color> Input: {combinedInput} looked for bit: {backBit}");
        //    nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        //    return;
        //}

        ApplyVelocity();

        if (dashTimer <= 0) {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }
    }

    private void ApplyVelocity() {
        int dashDir = Context._player.rev ? -1 : 1;
        Context.customRb.velocity.x = dashDir * (Context._moveSpeed * 2.5f);
    }

    public override void ExitState() {
        Context.customRb.velocity.x = 0f;
        //nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }

    public override PlayerStateMachine.EPlayerState GetNextState() => nextStateKey;

    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}
}