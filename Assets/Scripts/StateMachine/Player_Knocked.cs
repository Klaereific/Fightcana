using UnityEngine;
using System;

public class Player_Knocked : PlayerState
{
    int _knockStun;
    public Player_Knocked(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enter hit state");
        _knockStun = Context._knockStun;
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Knocked;
    }
    public override void UpdateState() {
        _knockStun --;
        if(_knockStun == 0){
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
