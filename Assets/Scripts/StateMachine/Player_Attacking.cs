//using UnityEngine;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine.Windows;
//public class Player_Attacking : PlayerState
//{
//    //PlayerStateMachine.EPlayerState nextStateKey; 
//
//
//    Vector2 position;
//    Vector2 size;
//
//    int startup;
//    int duration;
//    int recovery;
//    float damage;
//    int frame_count;
//    int hitstun;
//    int blockstun;
//    float hitForce;
//    float blockForce;
//
//    
//
//    PlayerStateMachine.Buttons button;
//
//    public Player_Attacking(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) : base(context, StateKey)
//    {
//        //PlayerStateContext Context = context;
//        //nextStateKey = StateKey;
//        frame_count = 0;
//    }
//
//    public override void EnterState()
//    {
//        Debug.Log($"Attack frame: {frame_count} / {startup + duration + recovery}");
//        frame_count = 0;
//        Context.customRb.velocity.x = 0;
//        // button = Context.button_queue.Dequeue();
//        // Attack attack = evaluateButton(button);
//        
//        // Debug.Log("MonoBehaviour Enabled: " + Context._buffer.enabled);
//        // Debug.Log(Context == null ? "Context is null" : "Context is not null");
//        Attack attack = EvaluateButtons1(Context._buffer_state);
//
//        Context.StopInputBuffer();
//        Context.isAttacking = false;
//        Context.ClearBufferState();
//
//        startup = attack._startup;
//        duration = attack._duration;
//        recovery = attack._recovery;
//        position = attack._position;
//        size = attack._size;
//        damage = attack._damage;
//        hitstun = attack._hitstun;
//        blockstun = attack._blockstun;
//        hitForce = attack._hitForce;
//        blockForce = attack._blockForce;
//
//        position = Context._player.rev ? new Vector2(attack._position.x * -1, attack._position.y) : attack._position; 
//        size = attack._size;
//
//        Context.animator.SetInteger("State", (int)StateKey);
//        Context.animator.SetInteger("Form", attack._animationForm);
//    }
//    public override void ExitState()
//    {
//        Context.StartInputBuffer();
//        Debug.Log("Exiting Attack State. Restarting Buffer.");
//        //nextStateKey = PlayerStateMachine.EPlayerState.Attacking;
//    }
//    public override void UpdateState()
//    {
//
//        Debug.Log($"Attack frame: {frame_count} / {startup + duration + recovery}");
//        frame_count += 1;
//        if (Context._isHit)
//        {
//            nextStateKey = PlayerStateMachine.EPlayerState.Hit;
//        }
//        if (frame_count == 1) {
//            //Debug.Log("Startup");
//            //Debug.Log(frame_count);
//            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, 0, 0 ,0 ,0, 0, (float)startup / 60f, Color.blue);
//        }
//        if (frame_count == (startup + 1)) {
//            //Debug.Log("Attack");
//            //Debug.Log(frame_count);
//            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, damage, blockstun, hitstun, blockForce,hitForce, (float)duration / 60f, Color.red);
//        }
//        if (frame_count == (duration + startup + 1))
//        {
//            //Debug.Log("Recovery");
//            //Debug.Log(frame_count);
//            PlayerStateMachine.SpawnHitbox(Context._hitboxPrefab, Context._player, (Vector2)Context.playerTransform.position + position, Context.playerTransform.rotation, size, 0, 0, 0, 0, 0, (float)recovery / 60f, Color.grey);
//        }
//        if(frame_count > (startup + duration + recovery))
//        {
//            if (Context._movementState == "Crouching")
//            {
//                nextStateKey = PlayerStateMachine.EPlayerState.Duck;
//            }
//            else
//            {
//                nextStateKey = PlayerStateMachine.EPlayerState.Idle;
//            }
//            Context.isAttacking = false;
//        }
//    }
//
//    public override PlayerStateMachine.EPlayerState GetNextState()
//    {
//        return nextStateKey;
//    }
//
//    public override void OnTriggerEnter(Collider other) { }
//    public override void OnTriggerStay(Collider other) { }
//    public override void OnTriggerExit(Collider other) { }
//
//    private Attack EvaluateButtons1(byte[][] bufferarray)
//    {
//        foreach (byte[] b in bufferarray)
//        {
//            //Debug.Log(b[0]);
//        }
//        int tail = bufferarray.GetLength(0) - 1;
//        //Debug.Log(bufferarray[tail][0]);
//
//        //if ((bufferarray[tail][0] & 0b10000000) != 0) { Debug.Log("Light"); }
//        //else if ((bufferarray[tail][0] & 0b01000000) != 0) { Debug.Log("Mid"); }
//        //else if ((bufferarray[tail][0] & 0b00100000) != 0) { Debug.Log("Heavy"); }
//        //else if ((bufferarray[tail][0] & 0b00010000) != 0) { Debug.Log("Special"); }
//
//        if ((bufferarray[tail][0] & InputButtons.LIGHT)!=0)
//        {
//            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gWest_attackDict, tail);
//            Debug.Log("Light");
//            Debug.Log(attack._name);
//            return (attack);
//        }
//        else if((bufferarray[tail][0] & InputButtons.MEDIUM)!= 0){
//            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gNorth_attackDict, tail);
//            Debug.Log("Mid");
//            Debug.Log(attack._name);
//            return (attack);
//        }
//        else if ((bufferarray[tail][0] & InputButtons.HEAVY) != 0)
//        {
//            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gEast_attackDict, tail);
//            Debug.Log("Heavy");
//            Debug.Log(attack._name);
//            return (attack);
//        }
//        else if ((bufferarray[tail][0] & InputButtons.SPECIAL) != 0)
//        {
//            Attack attack = EvaluateButtons2(bufferarray, Context._p1_CP.gSouth_attackDict, tail);
//            Debug.Log("Special");
//            Debug.Log(attack._name);
//            return (attack);
//        }
//        else
//        {
//            Debug.Log("Undefined Attack");
//            return (new Attack());
//        }
//    }
//    private Attack EvaluateButtons2(byte[][] buffer, Attack[] attackDict, int tail)
//    {
//        //Debug.Log("EB2 called");
//        foreach(Attack entry in attackDict)
//        {
//            Attack attack = entry;
//            //Debug.Log(attack._name);
//            
//            int pat_len = attack._inputs.Length;
//            //Debug.Log("This attacks pattern length is:"+pat_len);
//            int cur_pat = pat_len - 1;
//            int tol_f = 0;
//            if (Context._player.rev == true)
//            {
//                Debug.Log("Rev");
//                for (int i = tail; i > tail - attack._inputWindow; i--)
//                {
//
//                    if (tol_f == 0) { cur_pat = pat_len - 1; }
//                    //Debug.Log(i);
//                    //Debug.Log(buffer[i][0].ToString());
//                    //Debug.Log(cur_pat);
//                    while ((buffer[i][0] & SwitchBitsIfOneIsActive(attack._inputs[cur_pat],1,3))!=0)
//                    {
//                        tol_f = attack._inputTolerance;
//                        if (cur_pat == 0) { return entry; }
//                        cur_pat--;
//
//                    }
//                    if (tol_f != 0) { tol_f--; }
//
//                }
//            }
//            else
//            {
//                Debug.Log("For");
//                for (int i = tail; i > tail - attack._inputWindow; i--)
//                {
//
//                    if (tol_f == 0) { cur_pat = pat_len - 1; }
//                    //Debug.Log(i);
//                    //Debug.Log(buffer[i][0].ToString());
//                    //Debug.Log(cur_pat);
//                    while ((buffer[i][0] & attack._inputs[cur_pat]) != 0)
//                    {
//                        tol_f = attack._inputTolerance;
//                        if (cur_pat == 0) { return entry; }
//                        cur_pat--;
//                    }
//                    if (tol_f != 0) { tol_f--; }
//
//                }
//            }
//
//        }
//        //Debug.Log("Empty path");
//        //
//        //return (new KeyValuePair<string,Attack> ("empty",new Attack(0, 1, new byte[1] { 0b00000000 }, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.3f), 2f, 1, 1, 1)));
//        return (new Attack());
//    }
//    static byte SwitchBitsIfOneIsActive(byte value, int bit1, int bit2)
//    {
//        // Check if exactly one of the bits is set
//        int mask1 = 1 << bit1;
//        int mask2 = 1 << bit2;
//        bool shouldSwitch = ((value & mask1) != 0) ^ ((value & mask2) != 0);
//
//        // If the condition is true, toggle the bits
//        if (shouldSwitch)
//            value ^= (byte)(mask1 | mask2); // XOR both bits simultaneously
//
//        return value;
//    }
//}
//

using UnityEngine;

public class Player_Attacking : PlayerState
{
    Vector2 position;
    Vector2 size;

    int startup;
    int active;
    int recovery;
    float damage;
    int frame_count;
    int hitstun;
    int blockstun;
    float hitForce;
    float blockForce;

    public Player_Attacking(PlayerStateContext context, PlayerStateMachine.EPlayerState StateKey) 
        : base(context, StateKey)
    {
        frame_count = 0;
    }

    public override void EnterState()
    {
        frame_count = 0;
        Context.customRb.velocity.x = 0;

        // Find the right attack from the buffer
        AttackData attack = EvaluateAttack(Context._buffer_state);

        Context.StopInputBuffer();
        Context.isAttacking = false;
        Context.ClearBufferState();

        if (attack == null)
        {
            Debug.LogWarning("No valid attack found — returning to Idle");
            nextStateKey = PlayerStateMachine.EPlayerState.Idle;
            return;
        }

        Debug.Log($"Entering Attack: {attack.attackName} | Frames: {attack.startup}s / {attack.active}a / {attack.recovery}r");

        // Read frame data from the ScriptableObject
        startup   = attack.startup;
        active    = attack.active;
        recovery  = attack.recovery;
        damage    = attack.damage;
        hitstun   = attack.hitstun;
        blockstun = attack.blockstun;
        hitForce  = attack.hitForce;
        blockForce = attack.blockForce;
        size      = attack.hitboxSize;

        // Flip hitbox position if facing left
        position = Context._player.rev 
            ? new Vector2(-attack.hitboxPosition.x, attack.hitboxPosition.y) 
            : attack.hitboxPosition;

        // Drive the animator
        Context.animator.SetInteger("State", (int)StateKey);
        Context.animator.SetInteger("Form", attack.animationForm);
    }

    public override void ExitState()
    {
        Context.StartInputBuffer();
        Debug.Log("Exiting Attack State.");
    }

    public override void UpdateState()
    {
        frame_count += 1;

        // Check if we got hit during the attack
        if (Context._isHit)
        {
            nextStateKey = PlayerStateMachine.EPlayerState.Hit;
            return;
        }

        // --- Startup phase: non-damaging hitbox (blue) ---
        if (frame_count == 1 && startup > 0)
        {
            PlayerStateMachine.SpawnHitbox(
                Context._hitboxPrefab, Context._player,
                (Vector2)Context.playerTransform.position + position,
                Context.playerTransform.rotation,
                size, 0, 0, 0, 0, 0,
                (float)startup / 60f, Color.blue);
        }

        // --- Active phase: damaging hitbox (red) ---
        if (frame_count == startup + 1)
        {
            PlayerStateMachine.SpawnHitbox(
                Context._hitboxPrefab, Context._player,
                (Vector2)Context.playerTransform.position + position,
                Context.playerTransform.rotation,
                size, damage, blockstun, hitstun, blockForce, hitForce,
                (float)active / 60f, Color.red);
        }

        // --- Recovery phase: non-damaging hitbox (grey) ---
        if (frame_count == startup + active + 1 && recovery > 0)
        {
            PlayerStateMachine.SpawnHitbox(
                Context._hitboxPrefab, Context._player,
                (Vector2)Context.playerTransform.position + position,
                Context.playerTransform.rotation,
                size, 0, 0, 0, 0, 0,
                (float)recovery / 60f, Color.grey);
        }

        // --- Attack finished ---
        if (frame_count > startup + active + recovery)
        {
            nextStateKey = (Context._movementState == "Crouching")
                ? PlayerStateMachine.EPlayerState.Duck
                : PlayerStateMachine.EPlayerState.Idle;
            Context.isAttacking = false;
        }
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return nextStateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

    // ─────────────────────────────────────────────
    //  ATTACK EVALUATION
    // ─────────────────────────────────────────────

    /// <summary>
    /// Determines which attack to perform based on the input buffer.
    /// Checks which button was pressed, gets the appropriate attack list
    /// for the current stance, then pattern-matches from top to bottom.
    /// </summary>
    private AttackData EvaluateAttack(byte[][] bufferArray)
    {
        if (bufferArray == null) return null;

        int tail = bufferArray.Length - 1;
        byte pressedByte = bufferArray[tail][0];

        // Determine which button was pressed (check from most to least specific)
        byte buttonBit = 0;
        if ((pressedByte & InputButtons.LIGHT) != 0)   buttonBit = InputButtons.LIGHT;
        else if ((pressedByte & InputButtons.MEDIUM) != 0)  buttonBit = InputButtons.MEDIUM;
        else if ((pressedByte & InputButtons.HEAVY) != 0)   buttonBit = InputButtons.HEAVY;
        else if ((pressedByte & InputButtons.SPECIAL) != 0) buttonBit = InputButtons.SPECIAL;

        if (buttonBit == 0)
        {
            Debug.LogWarning("EvaluateAttack: No button bit found in press byte");
            return null;
        }

        // Get the attack list for this button + current stance
        string stance = Context._movementState; // "Grounded", "InAir", "Crouching"
        AttackData[] attacks = Context._p1_CP.moveset.GetAttacksForButton(buttonBit, stance);

        if (attacks == null || attacks.Length == 0)
        {
            Debug.LogWarning($"No attacks defined for button {buttonBit} in stance {stance}");
            return null;
        }

        // Try each attack in priority order (specials first, normals last)
        foreach (AttackData attack in attacks)
        {
            if (attack == null || attack.compiledInputs == null) continue;

            if (MatchInputPattern(bufferArray, attack, tail))
            {
                Debug.Log($"Matched: {attack.attackName}");
                return attack;
            }
        }

        Debug.LogWarning("No attack pattern matched");
        return null;
    }

    /// <summary>
    /// Checks if the input buffer matches this attack's compiled input pattern.
    /// Scans backwards through the buffer looking for the motion sequence.
    /// </summary>
    private bool MatchInputPattern(byte[][] buffer, AttackData attack, int tail)
    {
        byte[] pattern = attack.compiledInputs;
        int patLen = pattern.Length;

        // Simple normal (1-element pattern = just the button): always matches
        if (patLen == 1)
        {
            return (buffer[tail][0] & pattern[0]) != 0;
        }

        // Motion input: scan backwards through the buffer
        int curPat = patLen - 1;
        int toleranceLeft = 0;
        bool isReversed = Context._player.rev;

        for (int i = tail; i > tail - attack.inputWindow && i >= 0; i--)
        {
            if (toleranceLeft == 0) curPat = patLen - 1;

            byte framePress = buffer[i][0];
            byte patternByte = isReversed 
                ? MirrorHorizontal(pattern[curPat]) 
                : pattern[curPat];

            while ((framePress & patternByte) != 0)
            {
                toleranceLeft = attack.inputTolerance;
                if (curPat == 0) return true;
                curPat--;
                patternByte = isReversed 
                    ? MirrorHorizontal(pattern[curPat]) 
                    : pattern[curPat];
            }

            if (toleranceLeft > 0) toleranceLeft--;
        }

        return false;
    }

    /// <summary>
    /// Mirrors horizontal directional bits for when the player is facing left.
    /// Swaps LEFT (bit 3) and RIGHT (bit 1).
    /// </summary>
    private static byte MirrorHorizontal(byte value)
    {
        bool hasLeft  = (value & InputButtons.LEFT) != 0;   // bit 3
        bool hasRight = (value & InputButtons.RIGHT) != 0;  // bit 1

        if (hasLeft == hasRight) return value; // both or neither — no swap needed

        // Toggle both bits to swap them
        value ^= InputButtons.LEFT | InputButtons.RIGHT;
        return value;
    }
}
