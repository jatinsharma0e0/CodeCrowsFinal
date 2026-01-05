using UnityEngine;

namespace MR.Game.Puzzle
{
    public abstract class PuzzleBase : MonoBehaviour
    {
        public bool isSolved { get; protected set; }

        // Called when the player interacts or activates the puzzle
        public abstract void TrySolve();

        // Optional: feedback when puzzle is solved
        protected virtual void OnPuzzleSolved()
        {
            Debug.Log("solved!");
        }
    }
}
