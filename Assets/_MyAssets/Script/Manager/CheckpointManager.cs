using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Checkpoints
{
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance { get; private set; }

        private Checkpoint activeCheckpoint;

        private PlayerMovement playerMovement;

        private Transform activeRespawnPoint;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            playerMovement = GetComponent<PlayerMovement>();
        }

        public void SetActiveCheckpoint(Checkpoint checkpoint)
        {
            // Deactivate old checkpoint
            if (activeCheckpoint != null && activeCheckpoint != checkpoint)
            {
                activeCheckpoint.DeactivateCheckpoint();
            }

            // Activate new one
            activeCheckpoint = checkpoint;
            activeRespawnPoint = checkpoint.transform;

            // Handle player death: respawn, game over, etc.
            if (playerMovement != null)
            {
                playerMovement.Respawn();
            }
        }

        public Vector3 GetRespawnPosition()
        {
            return activeRespawnPoint != null  ? activeRespawnPoint.position : Vector3.zero;
        }
    }
}
