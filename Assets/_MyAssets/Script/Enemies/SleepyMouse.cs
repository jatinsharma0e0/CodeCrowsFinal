using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class SleepyMouse : Enemy
    {
        [Header("Waypoint Patrol")]
        public Transform[] waypoints;   // Assign empty GameObjects here
        private int currentIndex = 0;
        private bool isWaiting = false;

        [Header("Pause Settings")]
        public float pauseDuration = 1.5f; // seconds to wait at each waypoint
        private Quaternion targetRotation;

        protected override void PatrolOrMove()
        {
            if (isWaiting || waypoints == null || waypoints.Length == 0) return;

            // Move towards the current waypoint
            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            // Check if reached the waypoint
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Start waiting and rotate
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
    }
}
