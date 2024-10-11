using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    private CircularBuffer<byte[]> buffer;
    private System.Timers.Timer timer;
    private byte inputByte;
    public Player _player;
    private int bufferRow;
    private int bufferRow_prev;
    public int n;
    private Coroutine bufferCoroutine;
    // Start is called before the first frame update
    public void InitializeBuffer(int size, Player player)
    {
        _player = player;
        //timer = new System.Timers.Timer(17);
        buffer = new CircularBuffer<byte[]>(size);
        byte[] empty_in = new byte[3];
        buffer.Enqueue(empty_in);
        n = size;
        //timer.AutoReset = true;
        //timer.Elapsed += (sender, e) => SFT();
        
    }
    public void StartBuffer()
    {
        bufferCoroutine = StartCoroutine(BufferRoutine());
        Debug.Log(bufferCoroutine != null);
        //timer.Start();
        Debug.Log("Start Buffer");
    }

    public void StopBuffer()
    {
        if (bufferCoroutine != null)
        {
            StopCoroutine(bufferCoroutine);
            bufferCoroutine = null;
        }
        //timer.Dispose();
    }

    private IEnumerator BufferRoutine()
    {
        while (true)
        {
            SFT();
            yield return new WaitForSeconds(0.0167f); // 17ms interval
        }
    }

    private void SFT()
    {
        //Debug.Log("SFT");
        
        inputByte = _player.GetInput();
        //Debug.Log(inputByte);
        //Debug.Log("SFT");
        UpdateBuffer(inputByte);
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
        /*{if ((int)inputByte != 0)
        {
            printBuffer();
        }
        }*/
    }

    public void printBuffer()
    {
        Debug.Log(buffer.ReturnBufferArray()[0]);
        
    }

    public byte[][] GetBufferArray()
    {
        return (buffer.ReturnBufferArray());
    }

    public byte[] GetCurrentFrame()
    {
        return buffer.GetCurrentFrame();
    }
    

}
