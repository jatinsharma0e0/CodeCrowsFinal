using UnityEngine;

namespace MR.Game.Platforms
{
    /// <summary>
    /// This is TwoWayTeleporter script that allows teleportation between two linked teleporters.
    /// </summary>
    public class TwoWayTeleporter : MonoBehaviour
    {
        [Tooltip("The other teleporter this one links to.")]
        public TwoWayTeleporter linkedTeleporter;

        [Tooltip("Optional effect prefab at teleport location.")]
        public GameObject teleportEffect;

        [Tooltip("Cooldown after teleporting (in seconds).")]
        public float cooldownTime = 1f;

        private bool isCooldown = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!linkedTeleporter || isCooldown) return;
            if (!other.CompareTag("Player")) return;

            StartCoroutine(TeleportWithCooldown(other.gameObject));
        }

        private System.Collections.IEnumerator TeleportWithCooldown(GameObject player)
        {
            // Visual effect at source
            if (teleportEffect)
                Instantiate(teleportEffect, transform.position, Quaternion.identity);

            // Start cooldown on both teleporters to prevent instant return
            isCooldown = true;
            linkedTeleporter.isCooldown = true;

            // Teleport player
            player.transform.position = linkedTeleporter.transform.position;

            // Optional effect at destination
            if (linkedTeleporter.teleportEffect)
                Instantiate(linkedTeleporter.teleportEffect, linkedTeleporter.transform.position, Quaternion.identity);

            // Wait for cooldown
            yield return new WaitForSeconds(cooldownTime);
            isCooldown = false;
            linkedTeleporter.isCooldown = false;
        }
    }
}
