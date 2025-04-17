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
        Debug.Log("Enter hit state");
        _hitStun = Context._hitStun;
        _hitForce = Context._hitForce;
        Context.animator.SetInteger("State", 4);
        Context.animator.SetInteger("Form", 2);

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