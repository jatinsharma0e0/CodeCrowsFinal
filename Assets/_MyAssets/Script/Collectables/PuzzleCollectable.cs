using MR.Game.Puzzle;
using UnityEngine;

namespace MR.Game.Collectable
{
    public class PuzzleCollectable : ParentCollectable
    {
        public CollectablesPuzzle puzzle; // Assign in Inspector or via code

        private void Awake()
        {
            puzzle = GetComponentInParent<CollectablesPuzzle>();
        }

        public override void Collect(GameObject collector)
        {
            base.Collect(collector);
            puzzle?.RegisterCollectablePickup(); // Notify puzzle
        }
    }
}