using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Windows;
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
    float hitForce;
    float blockForce;

    private Attack activeAttack; 

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
        Context.customRb.velocity.x = 0;
        // button = Context.button_queue.Dequeue();
        // Attack attack = evaluateButton(button);
        
        // Debug.Log("MonoBehaviour Enabled: " + Context._buffer.enabled);
        // Debug.Log(Context == null ? "Context is null" : "Context is not null");


        activeAttack = EvaluateButtons1(Context._buffer_state);

        if (activeAttack == null)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            return; 
        }

        nextStateKey = PlayerStateMachine.EPlayerState.Attacking;

        Context._hitbox.ResetHitbox();
        Context.StopInputBuffer();
        Context.isAttacking = false;
        Context.ClearBufferState();

        if(activeAttack != null)
        {
            Context._hitbox.ResetHitbox();

            Context._hitbox.damage = activeAttack._damage;
            Context._hitbox.blockForce = activeAttack._blockForce;
            Context._hitbox.blockstun = activeAttack._blockstun;
            Context._hitbox.hitstun = activeAttack._hitstun;
            Context._hitbox.hitForce = activeAttack._hitForce;

            //startup = attack._startup;
            //duration = attack._duration;
            //recovery = attack._recovery;
            //position = attack._position;
            //size = attack._size;
            //damage = attack._damage;
            //hitstun = attack._hitstun;
            //blockstun = attack._blockstun;
            //hitForce = attack._hitForce;
            //blockForce = attack._blockForce;


        }
        

        //position = Context._player.rev ? new Vector2(attack._position.x * -1, attack._position.y) : attack._position; 
        //size = attack._size;
//
        //Context.animator.SetInteger("State", (int)StateKey);
        //Context.animator.SetInteger("Form", attack._animationForm);
        Context.animator.Play("5L", 0, 0f);    
        Debug.Log("Playing Animation: 5L");
    }
    public override void ExitState()
    {
        Context.StartInputBuffer();
        Debug.Log("Exiting Attack State. Restarting Buffer.");
        //nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
    }
    public override void UpdateState()
    {
        //frame_count += 1;
        //if (Context._isHit)
        //{
        //    nextStateKey = PlayerStateMachine.EPlayerState.Hit;
        //}
        //if (frame_count == 1) {
        //    //Debug.Log("Startup");
        //    //Debug.Log(frame_count);
        //    PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, 0, 0 ,0 ,0, 0, (float)startup / 60f, Color.blue);
        //}
        //if (frame_count == (startup + 1)) {
        //    //Debug.Log("Attack");
        //    //Debug.Log(frame_count);
        //    PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, damage, blockstun, hitstun, blockForce,hitForce, (float)duration / 60f, Color.red);
        //}
        //if (frame_count == (duration + startup + 1))
        //{
        //    //Debug.Log("Recovery");
        //    //Debug.Log(frame_count);
        //    PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, 0, 0, 0, 0, 0, (float)recovery / 60f, Color.grey);
        //}
        //if(frame_count > (startup + duration + recovery))
        //{
        //    if (Context._movementState == "Crouching")
        //    {
        //        nextStateKey = PlayerStateMachine.EPlayerState.Duck;
        //    }
        //    else
        //    {
        //        nextStateKey = PlayerStateMachine.EPlayerState.Idle;
        //    }
        //    Context.isAttacking = false;
        //}

        frame_count++;

        if (activeAttack == null) return;

        // PHASE 1: STARTUP -> ACTIVE
        // When we reach the startup frame, move and turn on the hitbox
        if (frame_count == activeAttack._startup)
        {
            // Position the hitbox based on your Attack data
            Context._hitbox.transform.localPosition = activeAttack._position;
            Context._hitbox.transform.localScale = activeAttack._size;
            Context._hitbox.gameObject.SetActive(true); 
        }

        if (frame_count >= activeAttack._startup && frame_count < activeAttack._startup + activeAttack._duration)
        {
            Context._hitbox.CheckForHits(); 
        }

        // PHASE 2: ACTIVE -> RECOVERY
        // When duration ends, turn off the hitbox
        if (frame_count == activeAttack._startup + activeAttack._duration)
        {
            Context._hitbox.gameObject.SetActive(false);
        }

        // PHASE 3: FINISH
        if (frame_count >= activeAttack._total_duration)
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
            //Debug.Log(b[0]);
        }
        int tail = bufferarray.GetLength(0) - 1;
        //Debug.Log(bufferarray[tail][0]);

        //if ((bufferarray[tail][0] & 0b10000000) != 0) { Debug.Log("Light"); }
        //else if ((bufferarray[tail][0] & 0b01000000) != 0) { Debug.Log("Mid"); }
        //else if ((bufferarray[tail][0] & 0b00100000) != 0) { Debug.Log("Heavy"); }
        //else if ((bufferarray[tail][0] & 0b00010000) != 0) { Debug.Log("Special"); }

        if ((bufferarray[tail][0] & 0b10000000)!=0)
        {
            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gWest_attackDict, tail);
            Debug.Log("Light");
            Debug.Log(attack._name);
            return (attack);
        }
        else if((bufferarray[tail][0] & 0b01000000)!= 0){
            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gNorth_attackDict, tail);
            Debug.Log("Mid");
            Debug.Log(attack._name);
            return (attack);
        }
        else if ((bufferarray[tail][0] & 0b00100000) != 0)
        {
            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gEast_attackDict, tail);
            Debug.Log("Heavy");
            Debug.Log(attack._name);
            return (attack);
        }
        else if ((bufferarray[tail][0] & 0b00010000) != 0)
        {
            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gSouth_attackDict, tail);
            Debug.Log("Special");
            Debug.Log(attack._name);
            return (attack);
        }
        else
        {
            Debug.Log("Undefined Attack");
            return null;
        }
    }
    private Attack EvaluateButtons2(byte[][] buffer, Attack[] attackDict, int tail)
    {
        Debug.Log("EB2 called");
        foreach(Attack entry in attackDict)
        {
            Attack attack = entry;
            //Debug.Log(attack._name);
            
            int pat_len = attack._inputs.Length;
            //Debug.Log("This attacks pattern length is:"+pat_len);
            int cur_pat = pat_len - 1;
            int tol_f = 0;
            if (Context._player.rev == true)
            {
                Debug.Log("Rev");
                for (int i = tail; i > tail - attack._inputWindow; i--)
                {

                    if (tol_f == 0) { cur_pat = pat_len - 1; }
                    //Debug.Log(i);
                    //Debug.Log(buffer[i][0].ToString());
                    //Debug.Log(cur_pat);
                    while ((buffer[i][0] & SwitchBitsIfOneIsActive(attack._inputs[cur_pat],1,3))!=0)
                    {
                        tol_f = attack._inputTolerance;
                        if (cur_pat == 0) { return entry; }
                        cur_pat--;

                    }
                    if (tol_f != 0) { tol_f--; }

                }
            }
            else
            {
                Debug.Log("For");
                for (int i = tail; i > tail - attack._inputWindow; i--)
                {

                    if (tol_f == 0) { cur_pat = pat_len - 1; }
                    //Debug.Log(i);
                    //Debug.Log(buffer[i][0].ToString());
                    //Debug.Log(cur_pat);
                    while ((buffer[i][0] & attack._inputs[cur_pat]) != 0)
                    {
                        tol_f = attack._inputTolerance;
                        if (cur_pat == 0) { return entry; }
                        cur_pat--;
                    }
                    if (tol_f != 0) { tol_f--; }

                }
            }

        }
        //Debug.Log("Empty path");
        //
        //return (new KeyValuePair<string,Attack> ("empty",new Attack(0, 1, new byte[1] { 0b00000000 }, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.3f), 2f, 1, 1, 1)));
        return null;
    }
    static byte SwitchBitsIfOneIsActive(byte value, int bit1, int bit2)
    {
        // Check if exactly one of the bits is set
        int mask1 = 1 << bit1;
        int mask2 = 1 << bit2;
        bool shouldSwitch = ((value & mask1) != 0) ^ ((value & mask2) != 0);

        // If the condition is true, toggle the bits
        if (shouldSwitch)
            value ^= (byte)(mask1 | mask2); // XOR both bits simultaneously

        return value;
    }
}
