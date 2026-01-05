using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class SlumberSnail : Enemy
    {
        [Header("Waypoint Patrol")]
        public Transform[] waypoints;
        private int currentIndex = 0;
        private bool isWaiting = false;

        [Header("Snail Settings")]
        public float pauseDuration = 1f;         // Pause at waypoints
        public GameObject slimePrefab;           // Prefab for slime hazard
        public float slimeDropInterval = 2f;     // How often to drop slime
        public float slimeLifetime = 5f;         // How long slime lasts
        private Quaternion targetRotation;

        private void Start()
        {
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;

            // Start dropping slime
            StartCoroutine(DropSlime());
        }

        protected override void PatrolOrMove()
        {
            if (isWaiting || waypoints.Length == 0) return;

            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                StartCoroutine(PauseAndRotate(target));
            }
        }

        private IEnumerator PauseAndRotate(Transform target)
        {
            isWaiting = true;
            yield return new WaitForSeconds(0.2f);

            // Calculate direction to next waypoint
            int nextIndex = (currentIndex + 1) % waypoints.Length;
            Vector3 direction = (waypoints[nextIndex].position - transform.position).normalized;

            // Rotate towards the next waypoint smoothly
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // only rotate on Y axis

                float t = 0;
                Quaternion startRot = transform.rotation;
                while (t < 1f)
                {
                    t += Time.deltaTime / 0.3f; // smooth rotation duration
                    transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
                    yield return null;
                }
            }

            // Switch to next waypoint
            currentIndex = (currentIndex + 1) % waypoints.Length;
            yield return new WaitForSeconds(pauseDuration);
            isWaiting = false;
        }

        private IEnumerator DropSlime()
        {
            while (true)
            {
                if (slimePrefab != null && !isWaiting)
                {
                    Vector3 dropPosition = transform.position + Vector3.down * 0.5f;
                    GameObject slime = Instantiate(slimePrefab, dropPosition, Quaternion.identity);

                    // Instead of instant destroy, tell slime to fade
                    if (slime.TryGetComponent<SlimeHazard>(out var hazard))
                    {
                        StartCoroutine(FadeAndDestroySlime(hazard, slimeLifetime));
                    }
                    else
                    {
                        Destroy(slime, slimeLifetime); // fallback
                    }
                }

                yield return new WaitForSeconds(slimeDropInterval);
            }
        }

        private IEnumerator FadeAndDestroySlime(SlimeHazard slime, float delay)
        {
            yield return new WaitForSeconds(delay);
            slime.StartFadeAndDestroy();
        }
    }
}
