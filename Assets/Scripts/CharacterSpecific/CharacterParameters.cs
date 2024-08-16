using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameters
{
    public float _weight;
    public float _walkspeed;
    public Vector2 _size;

    public Dictionary<string, Attack> attackDict = new Dictionary<string, Attack>();
    
    public CharacterParameters(float weight, float walkspeed, Vector2 size)
    {
        _weight = weight;
        _walkspeed = walkspeed;
        _size = size;
        attackDict.Add("idle_light", new Attack(new Vector2(0.5f, 0f), new Vector2(0.5f, 0.3f), 2f, 10, 20, 10));
        attackDict.Add("test", new Attack(new Vector2(0.0f, 0f), new Vector2(0.2f, 0.1f), 2f, 5, 5, 5));
    }
}
