using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{ 

    public Vector2 _position;
    public Vector2 _size;

    public float _damage;

    public int _startup;
    public int _duration;
    public int _recovery;

    public Attack(Vector2 position, Vector2 size, float damage ,int startup, int duration,int recovery)
    {
        _position = position;
        _size = size;

        _damage = damage;

        _startup = startup;
        _duration = duration;
        _recovery = recovery;
    }

}
