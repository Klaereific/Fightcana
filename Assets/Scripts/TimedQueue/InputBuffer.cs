using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameHandler;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class InputBuffer : MonoBehaviour
{
    public delegate void ButtonInputHandler(object source, byte[][] buffer_state);

    public event ButtonInputHandler OnButtonInput;

    private CircularBuffer<byte[]> buffer;
    private System.Timers.Timer timer;
    private byte inputByte;
    public Player _player;
    private int bufferRow;
    private int bufferRow_prev;
    public int n;
    private Coroutine bufferCoroutine;
    private MonoBehaviour coroutineExecutor;
    // Start is called before the first frame update

    private float _lastInputTime;

    public void InitializeBuffer(int size, Player player)
    {
        _player = player;
        //timer = new System.Timers.Timer(17);
        buffer = new CircularBuffer<byte[]>(size);
        byte[] empty_in = new byte[3];
        for (int i = 0; i < size; i++)
        {
            buffer.Enqueue(empty_in);
        }
        //buffer.Enqueue(empty_in);
        n = size;
        //timer.AutoReset = true;
        //timer.Elapsed += (sender, e) => SFT();
        
    }
    //public void StartBuffer(MonoBehaviour executor)
    //{
    //    if (executor == null)
    //    {
    //        Debug.LogError("InputBuffer: Executor is null! Buffer cannot start.");
    //        return;
    //    }
    //
    //    // Ensure we don't start it twice
    //    //StopAllCoroutines(); 
    //    executor.StartCoroutine(SFT());
//
    //    Debug.Log("InputBuffer: Started successfully.");
    //}
//
    //public void StopBuffer()
    //{
    //    if (bufferCoroutine != null && coroutineExecutor != null)
    //    {
    //        coroutineExecutor.StopCoroutine(bufferCoroutine);
    //        bufferCoroutine = null;
    //        coroutineExecutor = null;
    //        Debug.Log("Input Buffer stopped");
    //    }
    //    //timer.Dispose();
    //}
//
    //private IEnumerator BufferRoutine()
    //{
    //    while (true)
    //    {
    //        SFT();
    //        yield return new WaitForSeconds(0.0167f); // 17ms interval
    //    }
    //}
//
    //private IEnumerator SFT() 
    //{
    //    // 2. Added a loop so this runs every frame of the game
    //    while (true) 
    //    {
    //        // 3. Changed 'return' to 'continue' so it just skips this frame 
    //        // instead of killing the whole coroutine
    //        if (Player.CardModifierHeld) 
    //        {
    //            yield return null; 
    //            continue;
    //        }
    //
    //        inputByte = _player.GetInput();
    //        UpdateBuffer(inputByte);
    //
    //        // 4. This tells Unity: "Wait until the next frame, then start the loop again"
    //        yield return null; 
    //    }
    //}

    // Add this inside InputBuffer.cs

    public void Tick()
    {
        // This is the logic previously handled by SFT()
        // It processes the current raw inputByte into the Press/Hold/Release format
        // and pushes it into the CircularBuffer.

        byte press = 0;
        byte hold = 0;
        byte rel = 0;

        byte[] prevFrame = buffer.GetCurrentFrame(); 
        byte press_prev = prevFrame[0];
        byte hold_prev = prevFrame[1];

        for (int i = 0; i < 8; i++)
        {
            bool pp_bit = ((press_prev >> i) & 1) == 1;
            bool hp_bit = ((hold_prev >> i) & 1) == 1;
            bool in_bit = ((inputByte >> i) & 1) == 1;

            if (in_bit)
            {
                if (hp_bit || pp_bit) hold = (byte)(hold | (1 << i));
                else press = (byte)(press | (1 << i));
            }
            else
            {
                if (hp_bit || pp_bit) rel = (byte)(rel | (1 << i));
            }
        }

        byte[] input = new byte[3];
        input[0] = press;
        input[1] = hold;
        input[2] = rel;

        if(press != 0 || hold != 0)
        {
            //Debug.Log($"Buffer Tick: Press={press}, hold={hold}");
        }

        buffer.Enqueue(input);

        // Trigger the attack event if a button (bits 4-7) was pressed
        if (press > 15)
        {
            OnButtonInput?.Invoke(this, buffer.ReturnBufferArray());
        }
    }

    private void UpdateBuffer(byte inputByte)
    {
        //Debug.Log("UpdateBuffer");
        byte press_prev = buffer.Peek()[0];
        byte hold_prev = buffer.Peek()[1];
        
        byte press = 0;
        byte hold = 0;
        byte rel = 0;

        for (int i = 7; i >= 0; i--)
        {
            bool pp_bit = ((press_prev >> i) & 1) == 1;
            bool hp_bit = ((hold_prev >> i) & 1) == 1;
            bool in_bit = ((inputByte >> i) & 1) == 1;

            if (in_bit)
            {
                if(hp_bit || pp_bit)
                {
                    hold = (byte)(hold | (1 << i));
                }
                else
                {
                    press = (byte)(press | (1 << i));
                }
            }
            else
            {
                if (hp_bit || pp_bit)
                {
                    rel = (byte)(rel | (1 << i));
                }
            }
        }
        byte[] input = new byte[3];
        input[0] = press;
        input[1] = hold;
        input[2] = rel;

        // This means that I create an array of arrays where each index contains an array of three ele
        // each element can be press, hold or release 
        buffer.Enqueue(input);
        if (press > 15)
        {
            OnButtonInput?.Invoke(this, buffer.ReturnBufferArray());
        }
        
    }

    public void printBuffer()
    {
        Debug.Log(buffer.GetCurrentFrame()[0]);
        
    }

    public byte[][] GetBufferArray()
    {
        return (buffer.ReturnBufferArray());
    }

    public byte[] GetCurrentFrame()
    {
        return buffer.GetCurrentFrame();
    }
    

    private Stopwatch _inputTimer = new Stopwatch();
    private long _lastProcessTick = 0;
    public void UpdateRawInput(Vector2 move)
    {
        float captureTime = Time.realtimeSinceStartup;

        byte b = 0;
        float threshold = 0.35f;

        if (move.y > threshold)  b = (byte)(b | InputButtons.UP);
        if (move.y < -threshold) b = (byte)(b | InputButtons.DOWN);
        if (move.x < -threshold) b = (byte)(b | InputButtons.LEFT);
        if (move.x > threshold)  b = (byte)(b | InputButtons.RIGHT);

        //long currentTick = _inputTimer.ElapsedMilliseconds;
        //long delta = currentTick - _lastProcessTick;
        //if (b != 0) // Only log when you are actually pressing a direction
        //{
        //    UnityEngine.Debug.Log($"Input Received. Time since last process: {delta}ms | Raw: {move}");
        //}
        if(move != Vector2.zero)
        {
            //Debug.Log($"Raw Move: {move} | resulting byte: {b}");
        }
        inputByte = b;
    }   

    public bool CheckDash(bool isFlipped)
    {
        byte[][] history = GetBufferArray();
        int head = history.Length - 1;
        byte forwardMask = InputButtons.GetForwardMask(isFlipped);

        if ((history[head][0] & forwardMask) == 0) return false;

        bool foundGap = false;
        for (int i = 1; i < 20; i++) 
        {
            int idx = head - i;
            if (idx < 0) break;

            byte combined = (byte)(history[idx][0] | history[idx][1]);

            if ((combined & forwardMask) == 0) {
                foundGap = true;
            }

            if (foundGap && (history[idx][0] & forwardMask) != 0) {
                Debug.Log("<color=green>REAL DASH MATCHED</color>");
                return true;
            }
        }
        return false;
    }

    public bool CheckBackDash(bool isFlipped)
    {
        byte[][] history = GetBufferArray();
        int head = history.Length - 1;
        byte backMask = InputButtons.GetBackMask(isFlipped);

        if ((history[head][0] & backMask) == 0) return false;

        bool foundGap = false;
        for (int i = 1; i < 20; i++) 
        {
            int idx = head - i;
            if (idx < 0) break;

            byte combined = (byte)(history[idx][0] | history[idx][1]);

            if ((combined & backMask) == 0) {
                foundGap = true;
            }

            if (foundGap && (history[idx][0] & backMask) != 0) {
                Debug.Log("<color=green>REAL BACK DASH MATCHED</color>");
                return true;
            }
        }
        return false;
    }


    public bool IsForwardPressed(byte pressByte, bool isFlipped) 
    {
        byte forwardBit = isFlipped ? InputButtons.LEFT : InputButtons.RIGHT;
        return (pressByte & forwardBit) != 0; 
    }
    //This is so that there is some leeway with presses. 

    public void DebugHistory(bool isFlipped) 
    {
        byte[][] history = GetBufferArray();
        int head = history.Length - 1;
        byte forwardMask = InputButtons.GetForwardMask(isFlipped);

        string debugStr = "--- Input History (Last 15 Frames) ---\n";
        for (int i = 0; i < 15; i++) 
        {
            int idx = head - i;
            if (idx < 0) break;

            byte press = history[idx][0];
            byte hold = history[idx][1]; 

            byte combined = (byte)(press | hold);
        
            string arrow = GetArrowDirection(combined); 
            debugStr += $"[f-{i:00}] Raw:{combined:X2} {arrow} (P:{press:X2} H:{hold:X2})\n";
        }
        Debug.Log(debugStr);
    }

    private string GetArrowDirection(byte input)
    {
        bool u = (input & 4) != 0;
        bool d = (input & 1) != 0;
        bool l = (input & 8) != 0;
        bool r = (input & 2) != 0;

        if (u && r) return "↗";
        if (u && l) return "↖";
        if (d && r) return "↘";
        if (d && l) return "↙";
        if (u) return "↑";
        if (d) return "↓";
        if (l) return "←";
        if (r) return "→";
        return "•";
    }
}



