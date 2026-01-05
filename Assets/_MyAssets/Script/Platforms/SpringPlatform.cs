using System.Collections;
using UnityEngine;

namespace MR.Game.Platforms
{
    // This class is intended
    public class SpringPlatform : MonoBehaviour
    {
        public float springForce = 15f; // Adjust for desired jump height
        public float cooldown = 0.5f;   // Prevents rapid re-triggering

        private bool isOnCooldown = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!isOnCooldown && other.CompareTag("Player"))
            {
                if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity
                    rb.AddForce(Vector3.up * springForce, ForceMode.VelocityChange);
                }
                StartCoroutine(SpringCooldown());
            }
        }

        private IEnumerator SpringCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }
    }
}