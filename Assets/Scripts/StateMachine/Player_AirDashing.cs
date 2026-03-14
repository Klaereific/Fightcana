using UnityEngine;

public class Player_Air_Dashing : PlayerState
{
    private float dashDuration = 0.20f; 
    private float dashTimer;
    private float DashYStart;
    public Player_Air_Dashing(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate) {}

    public override void EnterState() 
    {
        Context._movementState = "AirDashing";
        Context.animator.SetInteger("State", 4); // Use unique ID if you have a separate air animation
        dashTimer = dashDuration;
        nextStateKey = StateKey; // CRITICAL: Stop instant exit
        ApplyVelocity();

        DashYStart = Context.customRb.position.y;

        Context.customRb.velocity.y = 0;
        ApplyVelocity();
    }
    
    public override void UpdateState() 
    {
        dashTimer -= Time.fixedDeltaTime;

        Vector2 lockedPos = Context.customRb.position;
        lockedPos.y = DashYStart;
        Context.customRb.position = lockedPos;

        ApplyVelocity(); 

        if (dashTimer <= 0) 
        {
            //Context.customRb.velocity.y = 0f;
            nextStateKey = PlayerStateMachine.EPlayerState.Jump;
            Context.customRb.velocity.y = 0f;
        }
    }

    private void ApplyVelocity() 
    {
        float dashDir = Context.FacingDirection;
        Context.customRb.velocity = new Vector2(dashDir * (Context._moveSpeed * 2.5f), 0f);
    }

    public override void ExitState() {
        Context.customRb.velocity.y = 0f;
        //Debug.Log("Killed Y velocity.");
        Context.customRb.velocity.x = 0f;
        //nextStateKey = PlayerStateMachine.EPlayerState.Idle;
    }

    public override PlayerStateMachine.EPlayerState GetNextState() => nextStateKey;

    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}
}