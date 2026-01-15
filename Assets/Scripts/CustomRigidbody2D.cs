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
    private Vector2 gravity = new Vector2(0, -30f);

    private GameObject _playerGo;
    private Player _player;

    public CustomRigidbody2D(float size_x, float size_y, float pos_x, float pos_y, GameObject playerGO,float margin)
    {
        
        size.x=size_x+(margin*2);
        size.y=size_y;
        position.x = pos_x;
        position.y = pos_y;
        _playerGo = playerGO;
        _player = _playerGo.GetComponent<Player>();
        BoxCollider2D BC = _playerGo.GetComponent<BoxCollider2D>();
        BC.size = size;
        BC.offset = new Vector2(0, size.y / 2);

    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }

    public void UpdatePhysics(float deltaTime)
    {
        // Apply gravity to velocity
        velocity += (gravity * gravityScale) * deltaTime;

        // Apply velocity to position
        position += velocity * deltaTime;

        // Collision checking (which snaps us back to ground)
        DetectAndResolveCollisions();
    }

    private void DetectAndResolveCollisions()
    {
        // Implement basic AABB collision detection and resolution with ground

        // min.y is now the feet (position.y)
        // max.y is the top of the head (position.y + height)
        Vector2 min = new Vector2(position.x - size.x / 2, position.y);
        Vector2 max = new Vector2(position.x + size.x / 2, position.y + size.y);
  
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
                    position.y = groundBounds.max.y; // + size.y / 2
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
                        position.x = wallBounds.min.x - size.x / 2;
                    }
                    if (min.x > wallBounds.min.x && min.x < wallBounds.max.x)
                    {
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