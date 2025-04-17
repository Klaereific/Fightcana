using UnityEngine;
using System;

public class Player_Blocking : PlayerState
{
    int _blockStun;
    Vector2 pushback;
    public Player_Blocking(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enter blocked state");
        _blockStun = Context._blockStun;
        Context.customRb.velocity.x = 0f;
        Context.animator.SetInteger("State", 4);
        if (Input.GetAxis(Context._player._MV_in) < -0.5f){
            Context.animator.SetInteger("Form", 1);
        }
        else{
            Context.animator.SetInteger("Form", 0);
        }
        pushback = new Vector2(Context._blockForce * (Context._player.rev ? 1 : -1), 0);

    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Blocking;
    }
    public override void UpdateState() {
        _blockStun --;
        Context.customRb.velocity = pushback;
        pushback *= 0.8f;
        if (pushback.magnitude < 0.2)
        {
            pushback = new Vector2(0, 0);
        }
        Debug.Log(_blockStun);
        if(_blockStun == 0){
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            Context._isBlocking = false;
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
