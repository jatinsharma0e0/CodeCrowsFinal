using UnityEngine;

namespace MR.Game.Intractable
{
    /// <summary>
    /// Represents a lever that can interact with a target door, toggling or opening it based on its configuration.
    /// </summary>
    /// <remarks>The lever can be configured to either toggle the door's state (open/close) or open it only
    /// once.  This behavior is controlled by the <see cref="toggle"/> field. The lever must be assigned a target  door
    /// via the <see cref="targetDoor"/> field for it to function correctly.</remarks>
    public class Lever : MonoBehaviour, IInteractable
    {
        public Door targetDoor; // Assign in Inspector

        public bool toggle = true; // If true, lever toggles door; if false, opens only once
        private bool hasActivated = false;
        public void OnInteract(GameObject interactor)
        {
            if (!toggle && hasActivated)
                return;

            if (targetDoor != null)
            {
                if (toggle)
                    targetDoor.ToggleDoor();
                else
                    targetDoor.OpenDoor();
            }

            hasActivated = true;
            // Optionally: play lever animation or sound here
            Debug.Log("Lever activated by " + interactor.name);
        }
    }
}
