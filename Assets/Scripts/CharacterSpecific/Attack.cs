using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct IntPair
{
    public int dir; // First integer
    public int button; // Second integer
}



[Serializable]
public class Attack
{

    public string _name;

    public string _animationKey;
    public int _animationForm;

    public Vector2 _position;
    public Vector2 _size;

    public float _damage;
    public int _blockstun;
    public int _hitstun;


    public int _startup;
    public int _duration;
    public int _recovery;

    public int _inputWindow;
    public int _inputTolerance;
    public IntPair[] _input_vir;
    [NonSerialized]
    public byte[] _inputs;
    public int _total_duration;
    
    public Attack(int inWin,int inTol,byte[] inBytes,Vector2 position, Vector2 size, float damage, int blockstun, int hitstun ,int startup, int duration,int recovery)
    {
        _inputWindow = inWin;
        _inputTolerance = inTol;
        _inputs = inBytes;


        _position = position;
        _size = size;

        _damage = damage;
        _blockstun = blockstun; 
        _hitstun = hitstun;

        _startup = startup;
        _duration = duration;
        _recovery = recovery;
        _inputs = Translate(_input_vir);

        _total_duration = startup + duration + recovery;
    }
    public Attack()
    {

    }
    private byte[] Translate(IntPair[] input_vir)
    {
        byte[] output=new byte[input_vir.Length];
        for (int i = 0; i < input_vir.Length; i++)
        {
            int dir = input_vir[i].dir;
            int button = input_vir[i].button;

            byte dir_b = 0b00000000;
            byte button_b = 0b00000000;

            if (dir == 1) { dir_b = 0b00001001; } // down left
            else if (dir == 2) { dir_b = 0b00000001; } // down center
            else if (dir == 3) { dir_b = 0b00000011; } // down right
            else if (dir == 4) { dir_b = 0b00001000; } // left
            else if (dir == 5) { dir_b = 0b00000000; } // center
            else if (dir == 6) { dir_b = 0b00000010; } // right
            else if (dir == 7) { dir_b = 0b00001100; } // up left
            else if (dir == 8) { dir_b = 0b00000100; } // up center
            else if (dir == 9) { dir_b = 0b00000110; } // up right

            if (button == 1) { button_b = 0b10000000; } // light (west)
            else if (button == 2) { button_b = 0b01000000; } // mid (north)
            else if (button == 3) { button_b = 0b00100000; } // heavy (east)
            else if (button == 4) { button_b = 0b00010000; } // special (south)

            output[i] = dir_b |= button_b;
            Debug.Log(output[i]);
        }
        return (output);
    }
    public void Init()
    {
        _inputs = Translate(_input_vir);
    }

}
