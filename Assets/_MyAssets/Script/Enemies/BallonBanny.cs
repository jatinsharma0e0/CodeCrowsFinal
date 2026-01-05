using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class BallonBanny : Enemy
    {
        [Header("Waypoint Patrol")]
        public Transform[] waypoints;
        private int currentIndex = 0;
        private bool isWaiting = false;

        [Header("Float Settings")]
        public float floatHeight = 0.5f;   // Bobbing distance
        public float floatSpeed = 2f;      // Bobbing speed
        public float pauseDuration = 1f;   // Pause at waypoint

        [Header("Projectile Drop")]
        public GameObject yarnBallPrefab;  // Projectile prefab
        public float dropInterval = 3f;    // How often to drop
        public float yarnLifetime = 5f;    // Destroy after seconds

        private Vector3 basePosition; // XZ position for patrol
        private float startY;         // Store initial Y position

        private void Start()
        {
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;

            basePosition = transform.position;
            startY = transform.position.y;

            StartCoroutine(DropYarnRoutine());
        }

        protected override void PatrolOrMove()
        {
            if (!isWaiting && waypoints.Length > 0)
            {
                Transform target = waypoints[currentIndex];

                // Move only in XZ plane
                Vector3 targetXZ = new(target.position.x, basePosition.y, target.position.z);
                basePosition = Vector3.MoveTowards(
                    basePosition,
                    targetXZ,
                    moveSpeed * Time.deltaTime
                );

                // Reached waypoint
                if (Vector3.Distance(basePosition, targetXZ) < 0.3f)
                {
                    StartCoroutine(PauseAtWaypoint());
                }
            }

            // Always apply bobbing on Y
            float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(basePosition.x, newY, basePosition.z);
        }

        private IEnumerator PauseAtWaypoint()
        {
            isWaiting = true;
            yield return new WaitForSeconds(pauseDuration);

            currentIndex = (currentIndex + 1) % waypoints.Length;
            Vector3 direction = waypoints[currentIndex].position - transform.position;
            RotateTowards(direction);

            isWaiting = false;
        }

        private IEnumerator DropYarnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(dropInterval);

                if (yarnBallPrefab != null)
                {
                    GameObject yarn = Instantiate(yarnBallPrefab, transform.position, Quaternion.identity);
                    Destroy(yarn, yarnLifetime);

                    Rigidbody rb = yarn.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.down * 5f; // Drop straight down
                    }
                }
            }
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }
        }
    }
}
