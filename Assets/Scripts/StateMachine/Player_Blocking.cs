using UnityEngine;
using System;

public class Player_Blocking : PlayerState
{
    int _blockStun;
    public Player_Blocking(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enter blocked state");
        _blockStun = Context._blockStun;
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Blocking;
    }
    public override void UpdateState() {
        _blockStun --;
        if(_blockStun == 0){
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
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
