using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class SlimeHazard : MonoBehaviour
    {
        [Header("Slime Settings")]
        public float slowAmount = 0.5f;    // Movement speed multiplier
        public int damage = 0;             // Optional damage over time
        public float damageInterval = 1f;  // Damage tick rate
        public float fadeDuration = 1.5f;  // Time to fade out before destroy

        private Renderer slimeRenderer;
        private Color originalColor;
        private bool isFading = false;

        private void Awake()
        {
            slimeRenderer = GetComponent<Renderer>();
            originalColor = slimeRenderer.material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<Player.PlayerMovement>(out var player))
                {
                    player.moveSpeed *= slowAmount; // slow down
                }

                if (damage > 0)
                    StartCoroutine(ApplyDamage(other));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<Player.PlayerMovement>(out var player))
                {
                    player.moveSpeed /= slowAmount; // restore speed
                }
            }
        }

        private IEnumerator ApplyDamage(Collider player)
        {
            Player.PlayerHealth health = player.GetComponent<Player.PlayerHealth>();
            while (player != null && health != null)
            {
                health.TakeDamage(damage);
                yield return new WaitForSeconds(damageInterval);
            }
        }

        /// <summary>
        /// Call this before destroying slime (e.g., from snail script)
        /// </summary>
        public void StartFadeAndDestroy()
        {
            if (!isFading)
                StartCoroutine(FadeOutAndDestroy());
        }

        private IEnumerator FadeOutAndDestroy()
        {
            isFading = true;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

                Color c = originalColor;
                c.a = alpha;
                slimeRenderer.material.color = c;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
