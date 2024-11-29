using UnityEngine;
using System;

public class Player_Hit : PlayerState
{
    int _hitStun;
    public Player_Hit(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enter hit state");
        _hitStun = Context._hitStun;
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Hit;
    }
    public override void UpdateState() {
        _hitStun --;
        Debug.Log(_hitStun);
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