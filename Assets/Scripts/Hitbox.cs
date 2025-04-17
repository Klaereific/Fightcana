using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;

public class Hitbox : MonoBehaviour
{


    public float damage = 10f;  // Damage dealt to the player
    public int hitstun = 0;
    public int blockstun = 0;
    public float hitForce = 0;
    public float blockForce = 0;
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

                opponent.TakeDamage(damage, hitstun, hitForce);
                hasHitPlayer = true;
            }
            if (opponent != null && opponent != sourcePlayer && opponent.isBlocking)
            {

                opponent.GoIntoBlock(blockstun, blockForce);
                hasHitPlayer = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (hitstun != 0 && blockstun != 0 && !hasHitPlayer)
        {
            DetectAndResolveCollisions();
        }
    }

    private void DetectAndResolveCollisions()
    {
        // Implement basic AABB collision detection and resolution with ground

        // Get the bounds of the player
        Vector2 min = transform.position - transform.localScale / 2;
        Vector2 max = transform.position + transform.localScale / 2;

        // Check for collisions with ground objects
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0.0f);
        Debug.Log("Collision");
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Get bounds of the ground object
                Player opponent = collider.gameObject.GetComponent<Player>();

                if (opponent != null && opponent != sourcePlayer)
                {
                    if (opponent.isBlocking)
                    {
                        opponent.GoIntoBlock(blockstun, blockForce);
                        hasHitPlayer = true;
                    }
                    else
                    {
                        opponent.TakeDamage(damage, hitstun, hitForce);
                        hasHitPlayer = true;
                    }
                }
            }
        }
    }
    //private void OnDestroy()

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = false;  // Reset the flag when the player leaves the hitbox
        }
    }
}
