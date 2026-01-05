using System.Collections;
using UnityEngine;

namespace MR.Game.Player
{
    /// <summary>
    /// Represents the health system for a player, including maximum health, current health, and methods to modify
    /// health.
    /// </summary>
    /// <remarks>This class provides functionality to manage a player's health, including increasing health,
    /// taking damage, and handling death when health reaches zero. The health system is initialized with a maximum
    /// health value, and the current health is set to the maximum health at the start.</remarks>
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 3;
        public int currentHealth;

        [Header("Damage Settings")]
        public float invincibilityDuration = 2f; // Time after taking damage
        public float blinkInterval = 0.1f;       // How fast the player blinks
        public Vector2 knockbackForce = new Vector2(5f, 5f); // X = horizontal, Y = vertical
        private bool isInvincible = false;

        private PlayerMovement playerMovement;
        private MeshRenderer meshRenderer;
        private Rigidbody rb;

        private void Awake()
        {
            currentHealth = maxHealth;
            playerMovement = GetComponent<PlayerMovement>();
            meshRenderer = GetComponent<MeshRenderer>();
            rb = GetComponent<Rigidbody>();
        }

        public void AddHealth(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            Debug.Log("Health increased! Current health: " + currentHealth);
            // Optionally update UI or play sound
        }

        public void TakeDamage(int amount)
        {
            if (isInvincible) return; // Prevent damage if invincible

            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0); // Prevent negative health
            Debug.Log("Player took damage! Current health: " + currentHealth);
            // Optionally update UI or play sound

            if (currentHealth <= 0)
            {
                Die();
            }

            else
            {
                // Apply knockback
                ApplyKnockback();
                // Start invincibility frames]  
                StartCoroutine(InvincibilityCoroutine());
                // Optional: add visual feedback like blinking here
            }
        }

        private void ApplyKnockback()
        {
            if (rb == null) return;

            // Determine knockback direction automatically
            float direction = 0f;

            // Try to get player's facing direction from movement script
            if (playerMovement != null)
            {
                direction = playerMovement.LastMoveDirection; // You can expose this in PlayerMovement
            }

            // Fallback: if no direction info, knockback left
            if (direction == 0f)
                direction = -1f;

            rb.linearVelocity = Vector2.zero; // Reset for consistency
            Vector2 force = new Vector2(knockbackForce.x * -direction, knockbackForce.y);
            rb.AddForce(force, ForceMode.Impulse);
        }

        private IEnumerator InvincibilityCoroutine()
        {
            isInvincible = true;
            float elapsed = 0f;

            while (elapsed < invincibilityDuration)
            {
                meshRenderer.enabled = !meshRenderer.enabled; // Toggle visibility
                yield return new WaitForSeconds(blinkInterval);
                elapsed += blinkInterval;
            }

            meshRenderer.enabled = true; // Make sure sprite is visible at the end
            isInvincible = false;
        }

        private void Die()
        {
            Debug.Log("Player died!");
            // Handle player death: respawn, game over, etc.
            if (playerMovement != null)
            {
                playerMovement.Respawn();
            }
            // Optionally reset health after respawn
            currentHealth = maxHealth;
        }
    }
}
