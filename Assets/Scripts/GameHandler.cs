using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameHandler : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, int oldHealth, int newHealth);
    public event HealthChangedHandler OnHealthChanged;

    public GameObject player1;
    
    [SerializeField]
    private int currentHealth;
    private int maxHealth = 100;

    public int testHeal = 5;
    public int testDamage = -5;

    public void ChangeHealth(int amount)
    {
        Debug.Log(amount);
        int oldHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);

        OnHealthChanged?.Invoke(player1,oldHealth,currentHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(testHeal);
        Debug.Log(testDamage);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            Debug.Log("Q: "+testHeal);
            ChangeHealth(testHeal);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("E: "+testDamage);
            ChangeHealth(testDamage);
        }
    }
}
