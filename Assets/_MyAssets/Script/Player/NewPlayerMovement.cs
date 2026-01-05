using UnityEngine;

namespace MR.Game.Player
{
    public class NewPlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        public float speed = 5f;
        public float jumpForce = 5f;
        public LayerMask groundLayer;

        private Vector3 respawnPosition;
        private int jumpCount;
        private bool isGrounded;

        Vector3 moveDirection;

        public float LastMoveDirection { get; private set; } = 1f;

        private PlayerController inputActions;
        private Vector2 moveInput;


        [Header("Player Getter Setter")]
        public int MaxJumps { get; private set; } = 1; // Start with single jump // Allows double jump

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputActions = new PlayerController();

            // Enable the input actions
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player.Jump.performed += ctx => Jump();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        private void FixedUpdate()
        {
            Move();
        }
        private void Move()
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * moveInput.y + camRight * moveInput.x;
            moveDirection.Normalize();

            rb.linearVelocity = moveDirection * speed + new Vector3(0, rb.linearVelocity.y, 0);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
            }
        }

        private void Jump()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.5f, groundLayer);
            Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red); // Show Ray in Scene
            Debug.Log("Is Grounded: " + isGrounded); // Check if ground detection is working

            if (isGrounded)
            {
                jumpCount = 0; // Reset jumps when on the ground
            }

            if (jumpCount < MaxJumps) // Allow double jump
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset fall speed
                rb.linearVelocity += Vector3.up * jumpForce; // Apply jump force
                jumpCount++; // Increase jump count
            }
        }

        public void Respawn()
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = respawnPosition;

            // Restore health to max on respawn
            if (TryGetComponent<PlayerHealth>(out var health))
            {
                health.currentHealth = health.maxHealth;
                // Optionally update UI or play a sound here
            }
        }
    }
}
