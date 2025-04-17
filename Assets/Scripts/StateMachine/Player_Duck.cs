using UnityEngine;

public class Player_Duck : PlayerState
{
    public Player_Duck(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        // PlayerStateContext Context = context;
    }

    public override void EnterState() {
        Debug.Log("Enter Ducked state");
        Context.customRb.velocity.x = 0f;
        Context._movementState = "Crouching";
        Context.animator.SetInteger("State", 3);
        if (!Context._isCrouching)
        {
            Duck();
        }
        
        
    }
    public override void ExitState() {
        nextStateKey = PlayerStateMachine.EPlayerState.Duck;
    }
    public override void UpdateState() {
        if (Context.isAttacking)
        {
            Context.isAttacking = false;
            nextStateKey = PlayerStateMachine.EPlayerState.Attacking;

        }
        if (Input.GetAxis(Context._player._MV_in) > -0.5f)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            Stand();
        }
        else if (Input.GetAxis(Context._player._MH_in) < 0.3f)
        {
            Context._player.isBlocking = true;
        }
        else
        {
            Context._player.isBlocking = true;
        }
    }
    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return nextStateKey;
    }
    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}
    public void Duck()
    {
        float duckedheight = Context._height / 2;
        Context.customRb.position = new Vector2(Context.playerTransform.position.x, Context.playerTransform.position.y - duckedheight / 2);
        Context.customRb.SetScale(Context._width, duckedheight);
        Context._isCrouching = true;
        //Context.playerTransform.localScale = new Vector2(Context._width, duckedheight);

    }
    public void Stand()
    {
        Context.customRb.position = Context.playerTransform.position;
        Context.customRb.SetScale(Context._width, Context._height);
        //Context.playerTransform.localScale = new Vector2(Context._width, Context._height);
        
    }
}
