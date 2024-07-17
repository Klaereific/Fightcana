using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject hitboxPrefab;  // Reference to the hitbox prefab
                                     // public Transform hitboxSpawnPoint;  // The position where the hitbox should spawn

    public int health = 100;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))  // Example attack input
        {
            Vector3 posOffset = new Vector3(0.5f, 0.5f);
            Debug.Log("Attack");
            SpawnHitbox(transform.position + posOffset,transform.rotation,10,0.1f);
        }
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnHitbox(Vector3 position, Quaternion rotation,int damage, float duration)
    {
        GameObject hitbox = Instantiate(hitboxPrefab, position, rotation);
        hitbox.GetComponent<Hitbox>().damage = damage;  // Set the damage value if needed
        Destroy(hitbox, duration);  // Destroy the hitbox after 0.5 seconds to simulate attack duration
    }
    


    public void TakeDamage(int damage)
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
