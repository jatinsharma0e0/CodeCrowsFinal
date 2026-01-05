using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public List<PuzzleBase> puzzles = new();
        public GameObject rewardObject; // Door, portal, etc.

        private bool rewardGiven = false;
        private void Start()
        {
            if (rewardObject != null)
            {
                rewardObject.SetActive(false); // Ensure the reward is initially hidden
            }
        }
        void Update()
        {
            if (!rewardGiven && AllPuzzlesSolved())
            {
                rewardGiven = true;
                GiveReward();
            }
        }

        public void RegisterPuzzle(PuzzleBase puzzle)
        {
            puzzles.Add(puzzle);
            Debug.Log($"✅ Registered Puzzle: {puzzle.name}, Total: {puzzles.Count}");
        }

        private bool AllPuzzlesSolved()
        {
            foreach (var puzzle in puzzles)
            {
                Debug.Log($"Checking {puzzle.name}, isSolved = {puzzle.isSolved}");
                if (!puzzle.isSolved)
                    return false;
            }
            return true;
        }

        private void GiveReward()
        {
            Debug.Log("🎉 All puzzles solved!");

            // Example: open a door or enable portal
            if (rewardObject != null)
            {
                rewardObject.SetActive(true);
            }
        }
    }
}
