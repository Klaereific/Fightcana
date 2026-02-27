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


    // CURRENT WORKING IMPLEMENTATION OF ENTER STATE BELOW


    //public override void EnterState()
    //{
    //    Context._movementState = "Jumping";
    //    Context.animator.SetInteger("State", 2);
//
    //    startHeight = Context.customRb.position.y;
//
    //    Context.customRb.velocity.y = Context._jumpForce;
//
    //    // Handle horizontal animation variants
    //    if (Mathf.Abs(Context.customRb.velocity.x) > 0.1f)
    //        Context.animator.SetInteger("Form", Context.customRb.velocity.x > 0 ? 2 : 3);
    //    else
    //        Context.animator.SetInteger("Form", 1);
    //}

    public override void EnterState()
    {
        Context._movementState = "Jumping";
        Context.animator.SetInteger("State", 2);
    
        startHeight = Context.customRb.position.y;
    
        // 1. ACTIVE INPUT CHECK: Look at the buffer to "lock in" the jump direction
        byte[] currentFrame = Context._buffer.GetCurrentFrame();
        byte input = (byte)(currentFrame[0] | currentFrame[1]); 
    
        bool isRight = (input & 0b00000010) != 0;
        bool isLeft  = (input & 0b00001000) != 0;
    
        float jumpDirection = 0;
        if (isRight) jumpDirection = 1;
        else if (isLeft) jumpDirection = -1;

        if (Context.HasAirDashed)
        {
            return;
        }
    
        // 2. SET VELOCITY: Apply vertical force AND the angled jump force
        Context.customRb.velocity.y = Context._jumpForce;
        
        // We set velocity.x here and never change it in UpdateState to make it "committed"
        Context.customRb.velocity.x = jumpDirection * Context._angledJump;
    
        // 3. ANIMATION: Use the jumpDirection we just calculated
        if (jumpDirection != 0)
            Context.animator.SetInteger("Form", jumpDirection > 0 ? 2 : 3);
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
