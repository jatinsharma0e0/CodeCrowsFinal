using MR.Game.Manager;
using MR.Game.Puzzle;
using UnityEngine;

namespace MR.Game.Collectable
{
    /// <summary>
    /// Represents a collectible coin in the game that can be collected by a player or other entity.
    /// </summary>
    /// <remarks>The coin rotates continuously around its Y-axis to provide a visual effect. When collected,
    /// it  increases the player's score by its assigned value.</remarks>
    public class Coin : ParentCollectable
    {
        public float rotationSpeed = 180f; // Degrees per second

        private void Update()
        {
            // Rotate around the Y axis
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up, Space.World);
        }
        public override void Collect(GameObject collector)
        {
            base.Collect(collector);
            GameManager.Instance.AddScore(value); // Add coin to score here
        }
    }
}