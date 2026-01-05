using MR.Game.Intractable;
using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Collectable
{
    public class DoubleJumpPowerUp : MonoBehaviour, IInteractable
    {
        public void OnInteract(GameObject interactor)
        {
            if (interactor.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                playerMovement.EnableDoubleJump();
                Debug.Log("Double Jump Power-Up collected!");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerMovement not found on interactor.");
            }
        }
    }
}
