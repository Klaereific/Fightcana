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
    //public Player sourcePlayer;

    public UnityEngine.Transform sourceTransform;
    /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitPlayer) return; // Exit if we've already hit something

        if (collision.gameObject.CompareTag("Player"))
        {
            Player opponent = collision.gameObject.GetComponent<Player>();

            if (opponent != null && opponent != sourcePlayer)
            {
                // Debug.Log($"HITBOX: Collided with {opponent.name}. Sending damage: {damage}, hitstun: {hitstun}");
                if (opponent.isBlocking)
                {
                    // Debug.Log("Hit was BLOCKED by " + opponent.name);
                    opponent.GoIntoBlock(blockstun, blockForce);
                }
                else
                {
                    // Debug.Log("Hit LANDED on " + opponent.name);
                    opponent.TakeDamage(damage, hitstun, hitForce);
                }

                if (sourcePlayer != null && sourcePlayer.cardManager != null)
                {
                    sourcePlayer.cardManager.AddMeterOnHitDealt();
                }
                Destroy(gameObject);
                //hasHitPlayer = true; 
            }
        }
    } */

    private void FixedUpdate()
    {
        Vector2 min = transform.position - transform.localScale / 2;
        Vector2 max = transform.position + transform.localScale / 2;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0.0f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Player opponent = collider.gameObject.GetComponent<Player>();

                // FIX 1A: Check if opponent exists AND opponent's transform is NOT the source transform
                if (opponent != null && opponent.transform != sourceTransform) 
                {
                    if (hasHitPlayer) continue; 

                    if (opponent.isBlocking)
                    {
                        opponent.GoIntoBlock(blockstun, blockForce);
                    }
                    else
                    {
                        opponent.TakeDamage(damage, hitstun, hitForce);
                    }

                    hasHitPlayer = true;
                }
            }
            //if (hitstun != 0 && blockstun != 0 && !hasHitPlayer)
            //{
            //    DetectAndResolveCollisions();
            //}
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
        //Debug.Log("Collision");
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Get bounds of the ground object
                Player opponent = collider.gameObject.GetComponent<Player>();

                if (opponent != null && opponent != sourceTransform)
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

    /* private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = false;  // Reset the flag when the player leaves the hitbox
        }
    } */
}
