using UnityEngine;

namespace MR.Game.Platforms.ElevatorPlatform
{
    /// <summary>
    /// Controls an elevator platform that moves between two points when the player enters and exits a trigger area.
    /// </summary>
    public class ElevatorPlatform : MonoBehaviour
    {
        public Transform startPoint;      // Assign in Inspector: where the elevator starts
        public Transform endPoint;        // Assign in Inspector: where the elevator goes
        public float speed = 2f;          // Movement speed

        private bool movingUp = false;
        private bool movingDown = false;

        private void FixedUpdate()
        {
            if (movingUp)
            {
                MoveToPosition(endPoint.position);
            }
            else if (movingDown)
            {
                MoveToPosition(startPoint.position);
            }
        }

        private void MoveToPosition(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                movingUp = true;
                movingDown = false;
                other.transform.parent = transform; // Attach player to platform
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                movingUp = false;
                movingDown = true;
                other.transform.parent = null; // Detach player when they leave
            }
        }
    }
}