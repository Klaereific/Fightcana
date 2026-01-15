using UnityEngine;
using System;

public class Player_Blocking : PlayerState
{
    int _blockStun;
    Vector2 pushback;

    private const byte BIT_DOWN  = 0b00000001; 
    private const byte BIT_RIGHT = 0b00000010; 
    private const byte BIT_UP    = 0b00000100;
    private const byte BIT_LEFT  = 0b00001000; 

    public Player_Blocking(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enter blocked state");
        _blockStun = Context._blockStun;

        Context.customRb.velocity.x = 0f;

        Context.animator.SetInteger("State", 4);

        byte holdByte = Context._buffer.GetCurrentFrame()[1];

        if((holdByte & BIT_DOWN) != 0)
        {
            Context.animator.SetInteger("Form", 1);
        }
        else
        {
            Context.animator.SetInteger("Form", 0);
        }

        float direction = Context._player.rev ? 1 : -1;
        pushback = new Vector2(Context._blockForce * direction, 0);
        //if (Input.GetAxis(Context._player._MV_in) < -0.5f){
        //    Context.animator.SetInteger("Form", 1);
        //}
        //else{
        //    Context.animator.SetInteger("Form", 0);
        //}
        //pushback = new Vector2(Context._blockForce * (Context._player.rev ? 1 : -1), 0);
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
        
        byte holdByte = Context._buffer.GetCurrentFrame()[1];
        bool isDown = (holdByte & BIT_DOWN) != 0;
        Context.animator.SetInteger("Form", isDown ? 1 : 0);

        if(_blockStun <= 0){

            bool holdingLeft = (holdByte & BIT_LEFT) != 0;
            bool holdingRight = (holdByte & BIT_RIGHT) != 0;

            bool isHoldingBack = (!Context._player.rev && holdingLeft) || (Context._player.rev && holdingRight);

            if (!isHoldingBack)
            {
                Context._isBlocking = false;
                nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            }
            else
            {
                nextStateKey = PlayerStateMachine.EPlayerState.Blocking;
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
