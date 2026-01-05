using MR.Game.Manager;
using UnityEngine;

namespace MR.Game.Traps
{
    public class SpikeTrap : Trap
    {
        public override void Activate(GameObject target)
        {
            base.Activate(target);
            Debug.Log("SpikeTrap: Player hit by spikes!");
            // Add specific logic, e.g., damage player or respawn
            GameManager.Instance.PlayerDied(); // Notify GameManager of player death
        }
    }
}
