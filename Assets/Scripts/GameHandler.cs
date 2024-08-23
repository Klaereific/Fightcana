using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameHandler : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, Player player, float oldHealth, float newHealth);
    public event HealthChangedHandler OnHealthChanged;
   
    

    public GameObject player1_GO;
    public GameObject player2_GO;

    private Player player1;
    private Player player2;

    [SerializeField]
    private int currentHealth_p1;
    private int currentHealth_p2;
    private int maxHealth = 100;

    public float max_distance;
    public float distance;
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
    private void Start()
    {
        max_distance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2;
        player1 = player1_GO.GetComponent<Player>();
        player2 = player2_GO.GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Sqrt(Mathf.Pow(player1_GO.transform.position.x - player2_GO.transform.position.x, 2));
        if (distance >= max_distance)
        {
            player1.x_axis_blocked = true;
            player2.x_axis_blocked = true;
        }
        else{
            player1.x_axis_blocked = false;
            player2.x_axis_blocked = false;
        }
        if((player1_GO.transform.position.x > player2_GO.transform.position.x) && !player1.rev)
        {
            player1.rev = true;
            player2.rev = false;
        }
        if((player1_GO.transform.position.x < player2_GO.transform.position.x) && player1.rev)
        {
            player1.rev = false;
            player2.rev = true;
        }
        //Debug.Log(player1.health);
        //Debug.Log(player2.health);
    }
}
