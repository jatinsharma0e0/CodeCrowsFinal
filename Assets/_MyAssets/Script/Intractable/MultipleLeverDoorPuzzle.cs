using MR.Game.Intractable;
using UnityEngine;

namespace MR.Game.Puzzle
{
    public class MultipleLeverDoorPuzzle : PuzzleBase
    {
        public PressurePlate[] pressurePlates;
        public Door rewardDoor; // Reference to your door script

        private bool puzzleSolved = false;

        private void Start()
        {
            // Optional: Close the door at start if needed
            if (rewardDoor != null && rewardDoor.isOpen)
            {
                rewardDoor.CloseDoor(); // Only if CloseDoor() is defined
            }
        }

        private void Update()
        {
            if (puzzleSolved || rewardDoor == null)
                return;

            foreach (var plate in pressurePlates)
            {
                if (!plate.IsPressed)
                    return;
            }

            // All plates are pressed
            puzzleSolved = true;
            Debug.Log("🎉 All pressure plates activated! Opening door...");
            rewardDoor.OpenDoor();
        }
        public override void TrySolve()
        {
            throw new System.NotImplementedException();
        }
    }
}
