using MR.Game.Intractable;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Puzzle
{
    public class PuzzleController : PuzzleBase
    {
        public List<PuzzleLevel> levers; // Assign all levers in the Inspector
        public Door targetDoor;    // Assign the door to open
        
        // Optionally, require all levers to be ON to open the door
        public override void TrySolve()
        {
            foreach (var lever in levers)
            {
                if (!lever.IsActivated)
                    return; // Puzzle not solved yet
            }
            targetDoor.OpenDoor();
            Debug.Log("Puzzle solved! Door opened.");

            if (!isSolved)
            {
                isSolved = true;
                Debug.Log($"{gameObject.name} solved!");
            }
        }
    }
}
