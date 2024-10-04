using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    private byte[,] buffer;
    System.Timers.Timer timer = new(interval: (1 / 60 * 1000));
    private byte inputByte;
    private Player _player;
    private int bufferRow;
    private int bufferRow_prev;
    int n;
    // Start is called before the first frame update
    public InputBuffer(int size, Player player)
    {
        n = size;
        buffer = new byte[size, 3];
        timer.Elapsed += (sender, e) => SFT();
        _player = player;
        bufferRow_prev = 0;
        bufferRow = 1;
    }
    public void StartBuffer()
    {
        timer.Start();
    }

    public void StopBuffer()
    {
        timer.Dispose();
    }
    
    private void SFT()
    {
        inputByte = _player.GetInput();
        UpdateBuffer(inputByte);
        bufferRow_prev++;
        bufferRow++;
        if (bufferRow_prev == n) { bufferRow_prev = 0; }
        if (bufferRow == n) { bufferRow = 0; }

    }

    private void UpdateBuffer(byte inputByte)
    {
        byte press_prev = buffer[bufferRow_prev, 1];
        byte hold_prev = buffer[bufferRow_prev, 2];
        
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
        buffer[bufferRow, 1] = press;
        buffer[bufferRow, 2] = hold;
        buffer[bufferRow, 3] = rel;

    }

    public byte[,] getBuffer()
    {
        return buffer;
    }
    

}
