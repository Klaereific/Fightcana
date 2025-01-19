using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CustomRigidbody2D
{
    public Vector2 position;
    public Vector2 velocity;
    public float gravityScale = 1.0f;
    public float mass = 1.0f;
    public Vector2 size = new Vector2(1.0f,1.0f);

    
    private Vector2 acceleration;
    private Vector2 gravity = new Vector2(0, -9.81f);

    private GameObject _playerGo;
    private Player _player;

    public CustomRigidbody2D(float size_x, float size_y, float pos_x, float pos_y, GameObject player,float margin)
    {
        
        size.x=size_x+(margin*2);
        size.y=size_y;
        position.x = pos_x;
        position.y = pos_y;
        _playerGo = player;
        _player = _playerGo.GetComponent<Player>();
    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }

    public void UpdatePhysics(float deltaTime)
    {
        // Apply gravity
        Vector2 gravityForce = gravity * gravityScale * deltaTime;
        ApplyForce(gravityForce);

        // Update velocity
        velocity += acceleration * deltaTime;

        // Update position
        position += velocity * deltaTime;

        // Reset acceleration
        acceleration = Vector2.zero;
        DetectAndResolveCollisions();
    }

    private void DetectAndResolveCollisions()
    {
        // Implement basic AABB collision detection and resolution with ground

        // Get the bounds of the player
        Vector2 min = position - size / 2;
        Vector2 max = position + size / 2;
  
        // Check for collisions with ground objects
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, 0.0f);
        foreach (var collider in colliders)
        {
            if(collider.CompareTag("Ground"))
            {
                // Get bounds of the ground object
                Bounds groundBounds = collider.bounds;

                // Simple resolution by setting the player on top of the ground
                if(velocity.y < 0 && max.y > groundBounds.min.y && min.y < groundBounds.max.y)
                {
                    position.y = groundBounds.max.y + size.y / 2;
                    //Debug.Log(size);
                    velocity.y = 0;
                }
            }
            if (collider.gameObject.CompareTag("Player") && collider.gameObject != _playerGo)
            {
                Bounds opponentBounds = collider.bounds;
                if (Mathf.Abs(velocity.x) > 0)
                {
                    if(max.x > opponentBounds.min.x && !_player.rev)
                    {
                        //Debug.Log("set to left");
                        position.x = opponentBounds.min.x - size.x / 2;
                    }
                    if(min.x < opponentBounds.max.x && _player.rev)
                    {
                        /*
                        Debug.Log("set to right");
                        Debug.Log(min.x);
                        Debug.Log(max.x);
                        Debug.Log(opponentBounds.min.x);
                        Debug.Log(opponentBounds.max.x);
                        */
                        position.x = opponentBounds.max.x + size.x / 2;
                    }
                    velocity.x = 0;
                }
            }
            if (collider.gameObject.CompareTag("Wall"))
            {
                Bounds wallBounds = collider.bounds;
                if (Mathf.Abs(velocity.x) > 0)
                {
                    if (max.x > wallBounds.min.x && max.x < wallBounds.max.x)
                    {
                        //Debug.Log("set to left");
                        position.x = wallBounds.min.x - size.x / 2;
                    }
                    if (min.x > wallBounds.min.x && min.x < wallBounds.max.x)
                    {
                        /*
                        Debug.Log("set to right");
                        Debug.Log(min.x);
                        Debug.Log(max.x);
                        Debug.Log(wallBounds.min.x);
                        Debug.Log(wallBounds.max.x);
                        */
                        position.x = wallBounds.max.x + size.x / 2;
                    }
                    velocity.x = 0;
                }
            }
        }
    }
    public void SetScale(float width, float height)
    {
        size.x = width;
        size.y = height;
    }
    public Vector2 GetScale()
    {
        return size;
    }
}