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
        Debug.Log("Collision");
        if (!hasHitPlayer && collision.gameObject.CompareTag("Player"))
        {
            Player opponent = collision.gameObject.GetComponent<Player>();

            if (opponent != null && opponent != sourcePlayer)
            {

                opponent.TakeDamage(damage,hitstun);
                hasHitPlayer = true;
            }
            if (opponent != null && opponent != sourcePlayer && opponent.isBlocking)
            {

                opponent.GoIntoBlock(blockstun);
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
