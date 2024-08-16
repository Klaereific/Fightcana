using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;  // Damage dealt to the player
    private bool hasDamagedPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDamagedPlayer && collision.gameObject.CompareTag("Dummy"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                hasDamagedPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dummy"))
        {
            hasDamagedPlayer = false;  // Reset the flag when the player leaves the hitbox
        }
    }
}
