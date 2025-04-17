using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameters : MonoBehaviour
{
    //public float _weight;
    //public float _walkspeed;
    //public Vector2 _size;

    
    [Header("Standing Attacks")]
    public Attack[] gWest_attackDict;
    public Attack[] gNorth_attackDict;
    public Attack[] gEast_attackDict;
    public Attack[] gSouth_attackDict;
    

    [Header("Aerial Attacks")]
    public Attack[] aWest_attackDict;
    public Attack[] aNorth_attackDict;
    public Attack[] aEast_attackDict;
    public Attack[] aSouth_attackDict; 
    

    [Header("Crouching Attacks")]
    public Attack[] cWest_attackDict;
    public Attack[] cNorth_attackDict;
    public Attack[] cEast_attackDict; 
    public Attack[] cSouth_attackDict;

    public CharacterParameters(float weight, float walkspeed, Vector2 size)
    {
        //_weight = weight;
        //_walkspeed = walkspeed;
        //_size = size;

        // byte[] light_attack = new byte[1] { 0b10000000 };
        // byte[] light_quci_f = new byte[4] { 0b00000001, 0b00000011, 0b00000010, 0b10000000 };
        // byte[] light_quci_b = new byte[4] { 0b00000001, 0b00001001, 0b00001000, 0b10000000 };
        
        // Attack( InputWindow[int], InputTolerance[int], InputBytes[byte[]], Position[Vector2], Size[Vector2], damage[float], blockstun[int], hitstun[int], startup[int], duration[int], recovery[int])
        
        // gWest_attackDict[0]=new Attack(30, 10, light_quci_f, new Vector2(2f, 0f),    new Vector2(1f, 1f),        2f, 25, 40, 25, 50, 30);
        // gWest_attackDict[1]=new Attack(30, 10, light_quci_b, new Vector2(0.5f, 1f),  new Vector2(0.2f, 1f),      5f, 40, 80, 30, 60, 20);
        // gWest_attackDict[2]=new Attack(3,   3,  light_attack, new Vector2(0.5f, 0.5f),new Vector2(1f, 0.3f),    5f, 40, 60, 10, 20, 10);
        
    }
    public void InitializeAttacks()
    {
        foreach(Attack attack in gWest_attackDict) { attack.Init(); }
        foreach (Attack attack in gEast_attackDict) { attack.Init(); }
        foreach (Attack attack in gSouth_attackDict) { attack.Init(); }
        foreach (Attack attack in gNorth_attackDict) { attack.Init(); }

        foreach (Attack attack in aWest_attackDict) { attack.Init(); }
        foreach (Attack attack in aEast_attackDict) { attack.Init(); }
        foreach (Attack attack in aSouth_attackDict) { attack.Init(); }
        foreach (Attack attack in aNorth_attackDict) { attack.Init(); }

        foreach (Attack attack in cWest_attackDict) { attack.Init(); }
        foreach (Attack attack in cEast_attackDict) { attack.Init(); }
        foreach (Attack attack in cSouth_attackDict) { attack.Init(); }
        foreach (Attack attack in cNorth_attackDict) { attack.Init(); }
    }
}
