using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{ 

    public Vector2 _position;
    public Vector2 _size;

    public float _damage=0;

    public int _startup = 0;
    public int _duration=0;
    public int _recovery=0;

    public int _inputWindow=0;
    public int _inputTolerance=0;
    public byte[] _inputs; 
    public Attack(int inWin,int inTol,byte[] inBytes,Vector2 position, Vector2 size, float damage ,int startup, int duration,int recovery)
    {
        _inputWindow = inWin;
        _inputTolerance = inTol;
        _inputs = inBytes;


        _position = position;
        _size = size;

        _damage = damage;

        _startup = startup;
        _duration = duration;
        _recovery = recovery;
    }
    public Attack()
    {

    }

}
