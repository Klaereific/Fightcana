using UnityEngine;

public class Player_Attacking : PlayerState
{
    //PlayerStateMachine.EPlayerState nextStateKey; 


    Vector2 position;
    Vector2 size;

    int startup;
    int duration;
    int recovery;
    float damage;
    int frame_count;

    PlayerStateMachine.Buttons button;

    public Player_Attacking(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
    {
        //PlayerStateContext Context = context;
        //nextStateKey = StateKey;
        frame_count = 0;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Attack state");
        frame_count = 0;
        // button = Context.button_queue.Dequeue();
        // Attack attack = evaluateButton(button);
        Attack attack = evaluateButtons(Context.button_queue);
        startup = attack._startup;
        duration = attack._duration;
        recovery = attack._recovery;
        position = attack._position;
        size = attack._size;
        damage = attack._damage;
        if (Context._player.rev)
        {
            position.x *= -1;
        }
    }
    public override void ExitState()
    {
        nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
    }
    public override void UpdateState()
    {
        frame_count += 1;
        if (frame_count == 1) {
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, 0, startup / 60f, Color.blue);
        }
        if (frame_count == (startup + 1)) {
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, damage, duration / 60f, Color.red);
        }
        if (frame_count == (duration + startup + 1))
        {
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, 0, recovery / 60f, Color.grey);
        }
        if(frame_count > (startup + duration + recovery))
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        }

    }
    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return nextStateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

    private Attack evaluateButton(TimedQueue button_queue)
    {

        if (button == PlayerStateMachine.Buttons.light_attack)
        {
            return (Context._p1_CP.attackDict["idle_light"]);
        }
        else
        {
            return (Context._p1_CP.attackDict["test"]);
        }
    }

}
