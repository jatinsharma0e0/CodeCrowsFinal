using UnityEngine;
using MR.Game.Player;

namespace MR.Game.Intractable
{
    /// <summary>
    /// Represents a door that can be opened, closed, or toggled between states.
    /// </summary>
    /// <remarks>The door's position transitions smoothly between its closed and open states. The open height
    /// and speed of the transition can be configured using the <see cref="openHeight"/> and <see cref="openSpeed"/>
    /// fields. Use the <see cref="OpenDoor"/>, <see cref="CloseDoor"/>, and <see cref="ToggleDoor"/> methods to control
    /// the door's state.</remarks>
    public class Door : MonoBehaviour, IInteractable
    {
        public bool isOpen = false;
        public float openHeight = 3f;
        public float openSpeed = 2f;

        private Vector3 closedPosition;
        private Vector3 openPosition;
        private Coroutine moveCoroutine;

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            closedPosition = transform.position;
            openPosition = closedPosition + Vector3.up * openHeight;
        }

        public void OpenDoor()
        {
            if (!isOpen)
            {
                isOpen = true;
                MoveDoor(openPosition);
            }
        }

        public void CloseDoor()
        {
            if (isOpen)
            {
                isOpen = false;
                MoveDoor(closedPosition);
            }
        }

        public void ToggleDoor()
        {
            if (isOpen)
                CloseDoor();
            else
                OpenDoor();
        }

        private void MoveDoor(Vector3 target)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveToPosition(target));
        }

        private System.Collections.IEnumerator MoveToPosition(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, openSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target;
        }

        public void OnInteract(GameObject interactor)
        {
            if (isOpen)
            {
                Debug.Log("Door is already open.");
                return;
            }

            if (interactor.TryGetComponent<PlayerKeyHolder>(out var keyHolder) && keyHolder.UseKey())
            {
                OpenDoor();
                Debug.Log("Door opened! Key used. Player now has " + keyHolder.keyCount + " key(s) left.");
                // Optionally: register this door as opened, play sound, etc.
            }
            else
            {
                Debug.Log("Door is locked. Player needs a key.");
            }
        }
    }
}
