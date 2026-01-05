using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Killzone
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerMovement>(out var player))
                {
                    player.Respawn();
                }
            }
        }
    }
} // namespace MR.Game.Killzone
