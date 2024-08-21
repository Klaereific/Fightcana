using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameHandler : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, Player player, float oldHealth, float newHealth);
    public event HealthChangedHandler OnHealthChanged;
   
    

    public Player player1;
    public Player player2;

    [SerializeField]
    private int currentHealth_p1;
    private int currentHealth_p2;
    private int maxHealth = 100;


    public void ChangeHealth(Player player, int amount)
    {
        Debug.Log(amount);
        float currentHealth = player.health;
        float oldHealth = player.health;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);

        OnHealthChanged?.Invoke(this,player, oldHealth,currentHealth);
     
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player1.health);
        //Debug.Log(player2.health);
    }
}
