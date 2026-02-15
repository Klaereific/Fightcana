using UnityEngine;

public class Player_BackDashing : PlayerState
{
    private float dashDuration = 0.15f; 
    private float dashTimer;

    public Player_BackDashing(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate) {}

    public override void EnterState() {
        Context._movementState = "BackDashing";
        Context.animator.SetInteger("State", 5); 
        dashTimer = dashDuration;
        nextStateKey = StateKey;
        ApplyVelocity();
    }
    
    public override void UpdateState() {
        dashTimer -= Time.fixedDeltaTime;

        ApplyVelocity();

        if (dashTimer <= 0) {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }
    }

    private void ApplyVelocity() {
        int dashDir = Context._player.rev ? 1 : -1;
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