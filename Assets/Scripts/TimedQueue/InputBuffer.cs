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

        if (move.y > 0.5f)  b |= 0b00000100; 
        if (move.y < -0.5f) b |= 0b00000001; 
        if (move.x < -0.5f) b |= 0b00001000; 
        if (move.x > 0.5f)  b |= 0b00000010; 

        //long currentTick = _inputTimer.ElapsedMilliseconds;
        //long delta = currentTick - _lastProcessTick;
        //if (b != 0) // Only log when you are actually pressing a direction
        //{
        //    UnityEngine.Debug.Log($"Input Received. Time since last process: {delta}ms | Raw: {move}");
        //}

        inputByte = b;
    }

    
   
}
