using UnityEngine;

namespace MR.Game.Intractable
{
    public class OneWayTeleportation : MonoBehaviour
    {
        public Transform teleportTarget; // Assign in Inspector

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && teleportTarget != null)
            {
                other.transform.position = teleportTarget.position;
                // Optionally reset velocity if using Rigidbody
                if (other.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                Debug.Log("Player teleported to: " + teleportTarget.position);
            }
        }
    }
}
