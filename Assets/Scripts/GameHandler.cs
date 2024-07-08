using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameHandler : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, int oldHealth, int newHealth);
    public event HealthChangedHandler OnHealthChanged;

    public GameObject player1;
    [Serialize Field]
    private int currentHealth;
    private int maxHealth = 100;

    public int testHeal = 5;
    public int testDamage = 5;

    public void ChangeHealth(int amount)
    {
        float oldHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);

        OnHealthChanged?.Invoke(player1,oldHealth,newHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            ChangeHealth(testHeal);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            ChangeHealth(testDamage);
        }
    }
}
