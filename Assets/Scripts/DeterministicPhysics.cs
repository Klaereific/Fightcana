using UnityEngine;

public class DeterministicPhysics : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 velocity)
    {
        rb.MovePosition(rb.position + velocity/*5 * Time.deltaTime*/);
    }
    
    public void ApplyVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
}