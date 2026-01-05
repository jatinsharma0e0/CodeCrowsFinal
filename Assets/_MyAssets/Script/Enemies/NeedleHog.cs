using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class NeedleHog : Enemy
    {
        [Header("Patrol Settings")]
        public Transform[] waypoints;
        public float patrolSpeed = 2f;
        private int currentIndex = 0;

        [Header("Charge Settings")]
        public float detectionRange = 8f;
        public float chargeSpeed = 12f;
        public float maxChargeDistance = 10f;
        public float recoveryTime = 2f;

        private Vector3 chargeStartPos;
        private bool isCharging = false;
        private bool isRecovering = false;
        private Transform player;
        private bool isWaiting;
        public float pauseDuration = 1f;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        protected override void Update()
        {
            if (isRecovering) return;

            if (isCharging)
            {
                ChargeForward();
            }
            else if (PlayerInRange())
            {
                StartCharge();
            }
            else
            {
                PatrolOrMove();
            }
        }

        protected override void PatrolOrMove()
        {
            if (waypoints.Length == 0) return;

            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                StartCoroutine(PauseAndRotate());
            }
        }

        private IEnumerator PauseAndRotate()
        {
            isWaiting = true;

            // Small pause before rotating
            yield return new WaitForSeconds(0.2f);

            // Calculate direction to next waypoint
            int nextIndex = (currentIndex + 1) % waypoints.Length;
            Vector3 direction = waypoints[nextIndex].position - transform.position;

            // Decide facing direction (only left or right)
            Quaternion targetRotation;
            if (direction.x > 0f)
                targetRotation = Quaternion.Euler(0, 0, 0);   // face right
            else
                targetRotation = Quaternion.Euler(0, 180, 0); // face left

            // Smoothly rotate to targetRotation
            Quaternion startRot = transform.rotation;
            float t = 0f;
            float rotateDuration = 0.3f; // time to rotate

            while (t < 1f)
            {
                t += Time.deltaTime / rotateDuration;
                transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
                yield return null;
            }

            // Switch waypoint index after rotation is done
            currentIndex = nextIndex;

            // Pause before moving again
            yield return new WaitForSeconds(pauseDuration);

            isWaiting = false;
        }

        private bool PlayerInRange()
        {
            if (player == null) return false;
            return Vector3.Distance(transform.position, player.position) <= detectionRange;
        }

        private void StartCharge()
        {
            isCharging = true;
            chargeStartPos = transform.position;
            RotateTowards(player.position);
        }

        private void ChargeForward()
        {
            transform.Translate(Vector3.forward * chargeSpeed * Time.deltaTime);

            if (Vector3.Distance(chargeStartPos, transform.position) >= maxChargeDistance)
            {
                EndCharge();
            }
        }

        private void EndCharge()
        {
            isCharging = false;
            isRecovering = true;
            Invoke(nameof(Recover), recoveryTime);
        }

        private void Recover()
        {
            isRecovering = false;
        }

        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isCharging)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(contactDamage);
                    }
                }

                // End charge on any obstacle
                EndCharge();
            }
        }
    }
}
