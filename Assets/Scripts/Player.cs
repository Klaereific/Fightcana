using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnHitHandler(object source, int hitstun);

    public event OnHitHandler OnHit;
    public event OnHitHandler OnBlock;


    public float health;
    public bool x_axis_blocked=false;
    public bool rev = false;
    public bool isBlocking = false;

    public string _W_in ="X1";
    public string _N_in = "Y1";
    public string _E_in = "B1";
    public string _S_in = "A1";

    public string _MV_in = "MoveVertical1";
    public string _MH_in = "MoveHorizontal";


    private void Awake()
    {
        health = 100f;
        
    }
    
    public void TakeDamage(float damage, int hitstun)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        Debug.Log("Player Health: " + health);
        OnHit?.Invoke(this, hitstun);
        if (health <= 0)
        {
            // Handle player death
            Debug.Log("Player is dead!");
        }
        
    }
    public void GoIntoBlock(int blockstun)
    {
        OnBlock?.Invoke(this, blockstun);
    }

    public byte GetInput()
    {
        byte input = 0;
        if (Input.GetButton(_W_in)) input |= 0b10000000; //West
        if (Input.GetButton(_N_in)) input |= 0b01000000; //North
        if (Input.GetButton(_E_in)) input |= 0b00100000; //East
        if (Input.GetButton(_S_in)) input |= 0b00010000; //South
        if (Input.GetAxis(_MH_in) < -0.5f) input |= 0b00001000; // Move left
        if (Input.GetAxis(_MV_in) > 0.5f) input |= 0b00000100; // Move up
        if (Input.GetAxis(_MH_in) > 0.5f) input |= 0b00000010; // Move right
        if (Input.GetAxis(_MV_in) < -0.5f) input |= 0b00000001; // Move down
        
        //Debug.Log(input);
        return input;
        
    }
    
}
