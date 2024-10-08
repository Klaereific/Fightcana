using UnityEngine;

public class Player : MonoBehaviour
{

    
    public float health;
    public bool x_axis_blocked=false;
    public bool rev = false;


    private void Awake()
    {
        health = 100f;
        
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        Debug.Log("Player Health: " + health);

        if (health <= 0)
        {
            // Handle player death
            Debug.Log("Player is dead!");
        }
    }
    public byte GetInput()
    {
        byte input = 0;
        if (Input.GetButton("X")) input |= 0b10000000; //West
        if (Input.GetButton("Y")) input |= 0b01000000; //North
        if (Input.GetButton("B")) input |= 0b00100000; //East
        if (Input.GetButton("A")) input |= 0b00010000; //South
        if (Input.GetAxis("MoveHorizontal") < -0.5f) input |= 0b00001000; // Move left
        if (Input.GetAxis("MoveVertical") > 0.5f) input |= 0b00000100; // Move up
        if (Input.GetAxis("MoveHorizontal") > 0.5f) input |= 0b00000010; // Move right
        if (Input.GetAxis("MoveVertical") < -0.5f) input |= 0b00000001; // Move down
        
        Debug.Log(input);
        return (byte)input;
        
    }
}
