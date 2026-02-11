using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    
    // Logic Variables
    [SerializeField] private float moveSpeed = 10f;
    private Animator animator;

    void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    // Use FixedUpdate for deterministic-style physics 
    // This runs exactly 50 or 60 times per second regardless of frame rate
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        float horizontalVelocity = moveInput.x * moveSpeed;


        transform.position += new Vector3(horizontalVelocity * Time.fixedDeltaTime, 0, 0);


        if (animator != null)
        {
            animator.SetFloat("MoveX", Mathf.Abs(moveInput.x));
        }
        
        // 4. CHARACTER FACING:
        if (moveInput.x > 0.1f) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (moveInput.x < -0.1f) transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
}