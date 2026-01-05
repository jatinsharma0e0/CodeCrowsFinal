using MR.Game.Puzzle;
using UnityEngine;

namespace MR.Game.Collectable
{
    /// <summary>
    /// Base class for all collectable items in the game.
    /// Override Collect() to define custom behavior per item.
    /// </summary>
    public class ParentCollectable : MonoBehaviour
    {
        public static ParentCollectable Instance;
        public int value = 1; // Example: value of the collectable

        /// <summary>
        /// Called when the player collects this item.
        /// Override this in a subclass to change behavior.
        /// </summary>
        /// <param name="collector">The GameObject collecting this item.</param>
        // Called when the player collects this item
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        public virtual void Collect(GameObject collector)
        {
            Debug.Log($"{gameObject.name} collected by {collector.name}!");
            // Common logic: play sound, animation, etc.
            Destroy(gameObject); // Remove collectable by default
        }

        /// <summary>
        /// Trigger detection for automatic collection when the player touches the item.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        // Optional: handle trigger
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }
    }
}