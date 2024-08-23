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
}
