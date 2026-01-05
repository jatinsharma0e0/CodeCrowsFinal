using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Collectable
{
    public class Health : ParentCollectable
    {
        /// <summary>
        /// Represents a health collectable that increases the player's health when collected.
        /// </summary>
        /// <remarks>This collectable does not rotate like coins, but can be customized further.</remarks>
        // No rotation needed for health collectables
        // Override Collect method to increase player's health
        // This method is called when the player collects this item
        public override void Collect(GameObject collector)
        {
            // Try to find a health component or method on the player
            if (collector.TryGetComponent<PlayerHealth>(out var health))
            {
                health.AddHealth(value); // Increase health by the collectable's value
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on collector.");
            }

            base.Collect(collector); // Call base logic (e.g., destroy object)
        }
    }
}
