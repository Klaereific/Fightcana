using UnityEngine;
using System;

public class Player_Hit : PlayerState
{
    int _hitStun;
    float _hitForce;
    Vector2 pushback;
    public Player_Hit(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        _hitStun = Context._hitStun;
        _hitForce = Context._hitForce;

        Debug.Log($"PLAYER_HIT: Entered hit state with {_hitStun} frames of hitstun.");

		Context.animator.SetInteger("State", 4);
		// Choose hit animation form by posture:
		// 0 = standing hit, 1 = crouch hit, 2 = air hit
		bool isCrouching = Context._movementState == "Crouching" || Input.GetAxis(Context._player._MV_in) < -0.5f;
		bool isAirborne = !Context.isGrounded;
		if (isCrouching)
		{
			Context.animator.SetInteger("Form", 1);
		}
		else if (isAirborne)
		{
			Context.animator.SetInteger("Form", 2);
		}
		else
		{
			Context.animator.SetInteger("Form", 0);
		}

        pushback = new Vector2(_hitForce * (Context._player.rev ? -1 : 1), 0);

        
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Hit;
    }
    public override void UpdateState() {
        _hitStun--;
        Context.customRb.velocity = pushback;
        pushback *= 0.8f;
        if (pushback.magnitude < 0.2)
        {
            pushback = new Vector2(0, 0);
        }
        if(_hitStun < 1){
            if(Context.isAttacking){
                nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
            }
            else{
                nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            }
            
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