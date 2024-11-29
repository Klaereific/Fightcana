using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{


    public float damage = 10f;  // Damage dealt to the player
    public int hitstun = 0;
    public int blockstun = 0;
    private bool hasHitPlayer = false;
    public Player sourcePlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision");
        if (!hasHitPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null && player != sourcePlayer)
            {
                
                player.TakeDamage(damage,hitstun);
                hasHitPlayer = true;
            }
            if (player != null && player != sourcePlayer && player.isBlocking)
            {

                player.GoIntoBlock(blockstun);
                hasHitPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = false;  // Reset the flag when the player leaves the hitbox
        }
    }
}
