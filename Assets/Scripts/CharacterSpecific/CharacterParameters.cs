using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameters
{
    public float _weight;
    public float _walkspeed;
    public Vector2 _size;

    public Dictionary<string, Attack> gWest_attackDict = new Dictionary<string, Attack>();
    
    public CharacterParameters(float weight, float walkspeed, Vector2 size)
    {
        _weight = weight;
        _walkspeed = walkspeed;
        _size = size;

        byte[] light_attack = new byte[1] { 0b10000000 };
        byte[] light_quci_f = new byte[4] { 0b00000001, 0b00000011, 0b00000010, 0b10000000 };
        byte[] light_quci_b = new byte[4] { 0b00000001, 0b00001001, 0b00001000, 0b10000000 };
        
        // Attack( InputWindow[int], InputTolerance[int], InputBytes[byte[]], Position[Vector2], Size[Vector2], damage[float], blockstun[int], hitstun[int], startup[int], duration[int], recovery[int])
        
        gWest_attackDict.Add("quci_f", new Attack(30, 10, light_quci_f, new Vector2(0.0f, 0f), new Vector2(0.2f, 0.1f), 2f, 25, 40, 10, 10, 10));
        gWest_attackDict.Add("quci_b", new Attack(30, 10, light_quci_b, new Vector2(0.0f, 0f), new Vector2(0.2f, 0.1f), 5f, 40, 80, 5, 5, 5));
        gWest_attackDict.Add("idle_light", new Attack(3,3,light_attack,new Vector2(0.5f, 0f), new Vector2(0.5f, 0.3f), 5f, 40, 60, 10, 20, 10));
        
    }
}
