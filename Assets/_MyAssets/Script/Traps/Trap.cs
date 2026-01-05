using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Traps
{
    public class Trap : MonoBehaviour
    {
        public int damage = 1;

        // Virtual method for triggering the trap
        public virtual void Activate(GameObject target)
        {
            Debug.Log("Trap activated!");
            // Common trap logic (e.g., play sound, animation)
        }

        // Optional: handle trigger
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}
