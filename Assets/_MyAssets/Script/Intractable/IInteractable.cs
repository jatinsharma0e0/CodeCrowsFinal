using UnityEngine;

namespace MR.Game.Intractable
{
    /// <summary>
    /// Represents an object that can be interacted with by a game entity.
    /// </summary>
    /// <remarks>Implement this interface to define custom interaction behavior for game objects. Interaction
    /// logic should be implemented in the <see cref="OnInteract(GameObject)"/> method.</remarks>
    public interface IInteractable
    {
        void OnInteract(GameObject interactor);
    }
}
