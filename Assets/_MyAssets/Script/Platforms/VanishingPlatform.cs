using System.Collections;
using UnityEngine;

namespace MR.Game.Platforms
{
    // This class is intended to handle the behavior of a vanishing platform in the game.
    // It will include functionality to make the platform disappear after a certain time or condition.
    public class VanishingPlatform : MonoBehaviour
    {
        public float vanishDelay = 0.5f;    // Time after player steps before vanishing
        public float reappearDelay = 2f;    // Time before platform reappears
        [Header("Materials")]
        public Material vanishMaterial;      // Assign a material for the vanish effect
        private Renderer platformRenderer;
        private Material originalMaterial;
        private Collider platformCollider;

        private void Start()
        {
            platformRenderer = GetComponent<Renderer>();
            platformCollider = GetComponent<Collider>();
            if (platformRenderer != null)
                originalMaterial = platformRenderer.material;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(VanishAndReappear());
            }
        }

        private IEnumerator VanishAndReappear()
        {
            // Change material before vanishing
            if (platformRenderer != null && vanishMaterial != null)
                platformRenderer.material = vanishMaterial;

            yield return new WaitForSeconds(vanishDelay);

            // Vanish: disable collider and renderer
            if (platformRenderer != null)
                platformRenderer.enabled = false;
            if (platformCollider != null)
                platformCollider.enabled = false;

            yield return new WaitForSeconds(reappearDelay);

            // Reappear: enable collider and renderer, restore original material
            if (platformRenderer != null)
            {
                platformRenderer.enabled = true;
                platformRenderer.material = originalMaterial;
            }
            if (platformCollider != null)
                platformCollider.enabled = true;
        }
    }
}