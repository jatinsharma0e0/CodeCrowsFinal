using MR.Game.Checkpoints;
using MR.Game.Intractable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MR.Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Movement")]
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public LayerMask groundLayer;
        private int jumpCount;

        [Header("Player Getter Setter")]
        public int MaxJumps { get; private set; } = 1; // Start with single jump // Allows double jump
        public float LastMoveDirection { get; private set; } = 1f;

        public float interactRange = 2f; // How far the player can interact

        private Rigidbody rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private PlayerController inputActions;
        public Vector2 CurrentMoveInput => moveInput;
        private Vector3 respawnPosition;

        // Start is called before the first frame update
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputActions = new PlayerController();
            jumpCount = 0; // Initialize jumps

            // Enable Inputs
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player.Jump.performed += ctx => Jump();

            // If you have an Interact action in your Input Actions, bind it here:
            inputActions.Player.Intraction.performed += ctx => Interact();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        private void Start()
        {
            // Set initial respawn position to the player's starting position
            respawnPosition = transform.position;
        }

        private void Update()
        {
            if (transform.position.y < -10f) // Adjust as needed
            {
                Respawn();
            }

            float move = Input.GetAxisRaw("Horizontal");

            if (move != 0)
                LastMoveDirection = move;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            Vector3 velocity = moveDirection * moveSpeed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            // Rotate player to face movement direction if moving
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f); // 0.2f controls rotation speed
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

        public void SetCheckpoint(Vector3 checkpointPosition)
        {
            respawnPosition = checkpointPosition;
        }

        public void Respawn()
        {
            rb.linearVelocity = Vector3.zero;
            Vector3 respawnPos = CheckpointManager.Instance.GetRespawnPosition();

            if (respawnPos != Vector3.zero)
                transform.position = respawnPos;

            // Restore health to max on respawn
            if (TryGetComponent<PlayerHealth>(out var health))
            {
                health.currentHealth = health.maxHealth;
                // Optionally update UI or play a sound here
            }
        }

        /// <summary>
        /// Attempts to interact with an object in front of the player.
        /// </summary>
        public void Interact()
        {
            Ray ray = new(transform.position + Vector3.forward * 0.5f, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.OnInteract(this.gameObject);
                    Debug.Log("Interacted with: " + hit.collider.name);
                }
                else
                {
                    Debug.Log("No interactable object in range.");
                }
            }
            else
            {
                Debug.Log("Nothing to interact with.");
            }
        }
        // ... (rest of your code)

        /// <summary>
        /// Call this method when the player acquires the double jump ability.
        /// </summary>
        public void EnableDoubleJump()
        {
            MaxJumps = 2;
            Debug.Log("Double Jump enabled!");
        }
    }
}