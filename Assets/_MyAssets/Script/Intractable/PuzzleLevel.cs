using MR.Game.Intractable;
using UnityEngine;

namespace MR.Game
{
    public class PuzzleLevel : MonoBehaviour, IInteractable
    {
        public bool IsActivated { get; private set; } = false;
        public Puzzle.PuzzleController puzzleController; // Assign in Inspector if part of a puzzle

        public void OnInteract(GameObject interactor)
        {
            IsActivated = !IsActivated; // Toggle lever state
            // Optionally: play animation or sound

            Debug.Log("Lever toggled by " + interactor.name + ". State: " + IsActivated);

            if (puzzleController != null)
                puzzleController.TrySolve();
        }
    }
}
