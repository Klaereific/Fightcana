using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;  // Damage dealt to the player
    private bool hasDamagedPlayer = false;
    public Player sourcePlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision");
        if (!hasDamagedPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null && player != sourcePlayer)
            {
                player.TakeDamage(damage);
                hasDamagedPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasDamagedPlayer = false;  // Reset the flag when the player leaves the hitbox
        }
    }
}
