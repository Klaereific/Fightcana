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
    public Player opponent;
    public static bool CardModifierHeld;

    public bool enableCardInput = true;

    private void Awake()
    {
        health = 100f;
        
    }
    
    public void TakeDamage(float damage, int hitstun, float hitForce)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        Debug.Log($"TAKEDAMAGE: Firing OnHit event with hitstun: {hitstun}");
        OnHit?.Invoke(this, hitstun, hitForce);                 // invokes method to transfer values to context
        if (cardManager != null) cardManager.AddMeterOnHitTaken();
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
    
    private int frameCounter = 0;

    void Update()
    {
        if (!enableCardInput) { return; }

        bool r1 = Input.GetKey(KeyCode.JoystickButton5);
        CardModifierHeld = r1;
        if (!r1) { frameCounter = 0; return; }

        frameCounter++;
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) {
            Debug.Log("R1 held");
        }

        if (cardManager == null) { return; }

        // Neutral R1: draw new hand only if meter is full and current hand is empty
        if (cardManager.hand.Count == 0 && Input.GetKeyDown(KeyCode.JoystickButton5)) {
            cardManager.TryDrawNewHand();
            return;
        }

        // Map specific joystick buttons to hand indices using GetKeyDown and hand-size guards
        if (cardManager.hand.Count > 0 && Input.GetKeyDown(KeyCode.JoystickButton0)) { UseCard(0); return; }
        if (cardManager.hand.Count > 1 && Input.GetKeyDown(KeyCode.JoystickButton1)) { UseCard(1); return; }
        if (cardManager.hand.Count > 2 && Input.GetKeyDown(KeyCode.JoystickButton2)) { UseCard(2); return; }
        if (cardManager.hand.Count > 3 && Input.GetKeyDown(KeyCode.JoystickButton3)) { UseCard(3); return; }

        // If your device maps face buttons to different indices, update the four lines above accordingly.
    }

    void UseCard(int cardIndex)
    {
        if (cardManager == null) { Debug.LogWarning("UseCard aborted: cardManager not assigned"); return; }
        if (cardIndex < 0 || cardIndex >= cardManager.hand.Count) { Debug.LogWarning("UseCard aborted: index out of range"); return; }

        CardData cardToUse = cardManager.hand[cardIndex];
        if (cardToUse == null) { Debug.LogWarning("UseCard aborted: null card at index"); return; }

        Player user = this;
        Player target = cardToUse.Target == CardTarget.Support ? this : opponent;

        Debug.Log($"UseCard idx={cardIndex}, handCount={cardManager.hand.Count}, card={cardToUse.name}, effect={(cardToUse.effect != null ? cardToUse.effect.name : "null")}, targetClass={cardToUse.Target}, target={(target != null ? target.name : "null")} ");

        if (cardToUse.effect != null)
            cardToUse.effect.Execute(user, target);

        cardManager.ConsumeHandToGraveyard();
    }
}
