using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    int hitstun;
    int blockstun;

    

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
        
        //Debug.Log("MonoBehaviour Enabled: " + Context._buffer.enabled);
        //Debug.Log(Context == null ? "Context is null" : "Context is not null");
        Attack attack = EvaluateButtons1(Context._buffer_state);
        startup = attack._startup;
        duration = attack._duration;
        recovery = attack._recovery;
        position = attack._position;
        size = attack._size;
        damage = attack._damage;
        hitstun = attack._hitstun;
        blockstun = attack._blockstun;
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
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, 0, 0 ,0 , startup / 60f, Color.blue);
        }
        if (frame_count == (startup + 1)) {
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, damage, blockstun, hitstun, duration / 60f, Color.red);
        }
        if (frame_count == (duration + startup + 1))
        {
            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, Context.customRb.position + position, Context.playerTransform.rotation, size, 0, 0, 0, recovery / 60f, Color.grey);
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

    private Attack EvaluateButtons1(byte[][] bufferarray)
    {
        foreach (byte[] b in bufferarray)
        {
            Debug.Log(b[0]);
        }
        int tail = bufferarray.GetLength(0) - 1;
        Debug.Log(bufferarray[tail][0]);
        
        if ((bufferarray[tail][0] & 0b10000000)!=0)
        {
            KeyValuePair<string, Attack>  attack = EvaluateButtons2(bufferarray, Context._p1_CP.gWest_attackDict, tail);
            Debug.Log(attack.Key);
            return (attack.Value);
        }
        else
        {
            Debug.Log("Undefined Attack");
            return (new Attack());
        }
    }
    private KeyValuePair<string, Attack> EvaluateButtons2(byte[][] buffer, Dictionary<string,Attack> attackDict, int tail)
    {
        foreach(KeyValuePair<string, Attack> entry in attackDict)
        {
            Attack attack = entry.Value;
            
            

            int pat_len = attack._inputs.Length;
            //Debug.Log("This attacks pattern length is:"+pat_len);
            int cur_pat = pat_len - 1;
            int tol_f = 0;
            for(int i = tail; i > tail - attack._inputWindow; i--)
            {
                
                if(tol_f == 0) { cur_pat = pat_len-1; }
                //Debug.Log(i);
                //Debug.Log(buffer[i][0].ToString());
                //Debug.Log(cur_pat);
                while ((buffer[i][0] & attack._inputs[cur_pat]) != 0) {
                    tol_f = attack._inputTolerance;
                    if (cur_pat == 0) { return entry; }
                    cur_pat--;
                }
                if (tol_f != 0) { tol_f--; }
                
            }
        }
        //Debug.Log("Empty path");
        //
        //return (new KeyValuePair<string,Attack> ("empty",new Attack(0, 1, new byte[1] { 0b00000000 }, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.3f), 2f, 1, 1, 1)));
        return (new KeyValuePair<string, Attack>("empty", new Attack()));
    }
}
