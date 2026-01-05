using MR.Game.Collectable;
using UnityEngine;

namespace MR.Game.Puzzle
{
    public class CollectablesPuzzle : PuzzleBase
    {
        public PuzzleCollectable[] collectables;
        public GameObject rewardObject; // e.g., door to open

        private bool puzzleStarted = false;
        private int collectedCount = 0;

        private void Awake()
        {
            // Find all PuzzleCollectable scripts in children and store them
            collectables = GetComponentsInChildren<PuzzleCollectable>();
        }

        protected void Start()
        {
            if (rewardObject != null)
            {
                rewardObject.SetActive(false); // Ensure the reward is initially hidden
            }
            // Hide collectables at first
            foreach (var item in collectables)
                item.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!puzzleStarted && other.CompareTag("Player"))
            {
                puzzleStarted = true;
                ActivateCollectables();
                Debug.Log("🧩 Puzzle Started: Collect 5 items.");
            }
        }

        void ActivateCollectables()
        {
            foreach (var item in collectables)
            {
                item.gameObject.SetActive(true);
            }
        }

        public void RegisterCollectablePickup()
        {
            collectedCount++;
            Debug.Log($"Collectable {collectedCount}/5");

            if (collectedCount == collectables.Length && !isSolved)
            {
                isSolved = true;
                GiveReward();
                OnPuzzleSolved();
            }
        }

        private void GiveReward()
        {
            if (rewardObject != null)
            {
                rewardObject.SetActive(true); // Example: open gate
                Debug.Log("🎁 Reward Unlocked!");
            }
        }

        public override void TrySolve()
        {
            if (!isSolved)
            {
                isSolved = true;
                Debug.Log($"{gameObject.name} solved!");
            }
        }
    }
}
