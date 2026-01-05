using UnityEngine;

namespace MR.Game.Puzzle
{
    public class MultiPlatePuzzleManager : MonoBehaviour
    {
        public PressurePlate[] pressurePlates;
        public GameObject rewardObject;

        private bool puzzleSolved = false;

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            if (rewardObject != null)
            {
                rewardObject.SetActive(false); // Ensure the reward is initially hidden
            }
        }

        void Update()
        {
            if (puzzleSolved) return;

            // Check if all plates are currently pressed
            foreach (var plate in pressurePlates)
            {
                if (!plate.IsPressed)
                    return; // Not solved yet
            }

            // All plates pressed
            puzzleSolved = true;
            Debug.Log("🎉 All pressure plates are active!");
            if (rewardObject != null)
                rewardObject.SetActive(true);
        }
    }
}
