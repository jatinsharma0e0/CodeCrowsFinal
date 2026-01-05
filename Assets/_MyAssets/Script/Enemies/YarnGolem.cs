using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class YarnGolem : Enemy
    {
        [Header("Waypoint Patrol")]
        public Transform[] waypoints;     // Empty GameObjects
        private int currentIndex = 0;

        [Header("Detection")]
        public float detectionRange = 5f; // Distance to start chasing
        public float attackRange = 1.2f;  // Distance to damage player
        public float attackCooldown = 1f; // Time between attacks
        private bool canAttack = true;

        private Transform player;
        private bool isChasing = false;
        private bool isWaiting = false;

        [Header("Pause Settings")]
        public float pauseDuration = 1f;

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        protected override void PatrolOrMove()
        {
            if (isWaiting) return;

            if (player == null)
            {
                Patrol();
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                isChasing = true;
                ChasePlayer();
            }
            else
            {
                isChasing = false;
                Patrol();
            }
        }

        private void Patrol()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            // Use a slightly bigger threshold (0.2f) to avoid precision issues
            if (Vector3.Distance(transform.position, target.position) < 1.2f && !isWaiting)
            {
                StartCoroutine(PauseAtWaypoint());
            }
        }

        private IEnumerator PauseAtWaypoint()
        {
            isWaiting = true;

            // Pause
            yield return new WaitForSeconds(pauseDuration);

            // Move to the next waypoint
            currentIndex = (currentIndex + 1) % waypoints.Length;

            // Rotate towards new waypoint
            Vector3 direction = waypoints[currentIndex].position - transform.position;
            RotateTowards(direction);

            isWaiting = false;
        }

        private void ChasePlayer()
        {
            if (player == null) return;

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime * 1.2f // Slightly faster than patrol
            );

            RotateTowards(player.position - transform.position);

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction.x > 0f)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (direction.x < 0f)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        private IEnumerator AttackPlayer()
        {
            canAttack = false;

            if (player != null)
            {
                if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamage(contactDamage);
                    Debug.Log($"{gameObject.name} attacked the player!");
                }
            }

            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
