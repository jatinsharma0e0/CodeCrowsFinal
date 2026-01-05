using MR.Game.Intractable;
using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Collectable
{
    public class Key : MonoBehaviour, IInteractable
    {
        public void OnInteract(GameObject interactor)
        {
            if (interactor.TryGetComponent<PlayerKeyHolder>(out var keyHolder))
            {
                keyHolder.AddKey();
                Debug.Log("Key collected! Player now has " + keyHolder.keyCount + " key(s).");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerKeyHolder not found on interactor.");
            }
        }
    }
}