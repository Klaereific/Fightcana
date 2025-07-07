using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnHitHandler(object source, int hitstun,float hitForce);

    public event OnHitHandler OnHit;
    public event OnHitHandler OnBlock;

    public float height = 2.0f;
    public float width = 1.0f;

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

    public CardManager cardManager; // Assign this in the Inspector or via script

    private void Awake()
    {
        health = 100f;
        
    }
    
    public void TakeDamage(float damage, int hitstun, float hitForce)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        Debug.Log("Player Health: " + health);
        OnHit?.Invoke(this, hitstun, hitForce);                 // invokes method to transfer values to context
        if (health <= 0)
        {
            // Handle player death
            Debug.Log("Player is dead!");
        }
        
    }
    public void GoIntoBlock(int blockstun, float blockForce)
    {
        OnBlock?.Invoke(this, blockstun, blockForce);           // invokes method to transfer values to context
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
    
    void Update()
    {
        bool r1 = Input.GetKey(KeyCode.JoystickButton5);

        if (r1 && Input.GetKeyDown(KeyCode.JoystickButton0)) {
            // R1 + Square
            UseCard(0);
        }
        if (r1 && Input.GetKeyDown(KeyCode.JoystickButton3)) {
            // R1 + Triangle
            UseCard(1);
        }
        if (r1 && Input.GetKeyDown(KeyCode.JoystickButton1)) {
            // R1 + X
            UseCard(2);
        }
        if (r1 && Input.GetKeyDown(KeyCode.JoystickButton2)) {
            // R1 + Circle
            UseCard(3);
        }
    }

    void UseCard(int cardIndex)
    {
        if (cardManager == null) return;
        if (cardIndex < 0 || cardIndex >= cardManager.hand.Count) return;

        Card cardToUse = cardManager.hand[cardIndex];

        // Do something with the card (e.g., apply its effect)
        Debug.Log("Used card: " + cardToUse.name);

        // Discard the card
        cardManager.DiscardCard(cardToUse);
    }
}
