using UnityEngine;
using System;

public class Player_Jump : PlayerState
{
    private float startHeight;
    private bool isFalling;
    public Player_Jump(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        //PlayerStateContext Context = context;
    }

    //public override void EnterState()
    //{
    //    //Debug.Log("Enter Jump state");
    //    float moveInput = Input.GetAxis(Context._player._MH_in);
    //    startHeight = Context.customRb.position.y;
    //    isFalling = false;
    //    Context._movementState = "Jumping";
    //    Context.animator.SetInteger("State", 2);
    //    if (Context.customRb.velocity.x > 0.1)
    //    {
    //        Context.animator.SetInteger("Form", 2);
    //    }
    //    else if (Context.customRb.velocity.x < -0.1)
    //    {
    //        Context.animator.SetInteger("Form", 3);
    //    }
    //    else
    //    {
    //        Context.animator.SetInteger("Form", 1);
    //    }
//
//
//
    //    Jump(moveInput, Context);
    //    //Context.jumpRequest = false;
    //    
    //}
    public override void EnterState()
    {
        Context._movementState = "Jumping";
        Context.animator.SetInteger("State", 2);

        // 1. Record the start height (now -0.5f)
        startHeight = Context.customRb.position.y;

        // 2. Set the initial upward burst. 
        // Do NOT add to velocity; set it directly to ensure consistency.
        Context.customRb.velocity.y = Context._jumpForce;

        // Handle horizontal animation variants
        if (Mathf.Abs(Context.customRb.velocity.x) > 0.1f)
            Context.animator.SetInteger("Form", Context.customRb.velocity.x > 0 ? 2 : 3);
        else
            Context.animator.SetInteger("Form", 1);
    }


    public override void ExitState() {
        Context.jumpRequest = false;
        nextStateKey = PlayerStateMachine.EPlayerState.Jump;
    }
    public override void UpdateState() {
        if (Context.customRb.velocity.y <= 0 && Context.customRb.position.y <= -0.49f)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }
        //else if (Context.customRb.velocity.y > 0 && Input.GetAxis(Context._player._MV_in)<0.5f)
        //{
        //    Context.customRb.velocity += Vector2.up * Physics2D.gravity.y * (Context._lowJumpMultiplier - 1) * Time.deltaTime;
        //}
        //if (isFalling && currHeight< 0.01f)
        //{
        //    nextStateKey= PlayerStateMachine.EPlayerState.Idle;
        //}
        //
        //else if (currHeight > 0.01f)
        //{
        //    Context.customRb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        //}
        /*{
        if(Context.button_queue.Count > 0)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.JumpAttack;
        }
        }*/
        
    }
    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return nextStateKey;
    }
    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}

    public void Jump(float moveInput,PlayerStateContext context)
    {
        //customRb.ApplyForce(new Vector2(0,jumpForce));
        if (Math.Abs(moveInput) < 0.01f)
        {
            context.customRb.velocity.y = context._jumpForce;
            //context.jumpRequest = false;
        }
        else
        {
            context.customRb.velocity.y = context._jumpForce;
            context.customRb.velocity.x = moveInput * context._angledJump;
            //jumpRequest = false;
        }
    }

}
