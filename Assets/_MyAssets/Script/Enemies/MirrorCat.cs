using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class MirrorCat : Enemy
    {
        [Header("Clone Settings")]
        public GameObject clonePrefab;
        public int cloneCount = 3;
        public float cloneLifetime = 6f;
        [Range(0f, 1f)]
        public float realCloneChance = 0.5f;

        [Header("Clone Behavior")]
        public bool isClone = false;
        [HideInInspector] public bool canBeDestroyed;

        [Header("Attack Settings (real only)")]
        public int attackDamage = 1;
        public float attackCooldown = 1.5f;
        private bool canAttack = true;

        [Header("AI Settings (real only)")]
        public float detectionRange = 8f;
        public float attackRange = 2f;
        private Transform player;

        [Header("Waypoint Patrol")]
        public Transform[] waypoints;       // Assigned in inspector
        public float waypointPause = 1f;    // Pause before moving
        private int currentIndex = 0;
        private bool isWaiting = false;

        private void Start()
        {
            if (!isClone)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                    player = playerObj.transform;

                if (waypoints.Length > 0)
                    transform.position = waypoints[0].position;
            }
            else
            {
                // Randomize clone destroyable flag
                canBeDestroyed = Random.value < realCloneChance;

                // Visual effect if indestructible
                if (!canBeDestroyed)
                {
                    Renderer rend = GetComponentInChildren<Renderer>();
                    if (rend != null)
                    {
                        Color c = rend.material.color;
                        c.a = 0.5f;
                        rend.material.color = c;
                    }
                }

                // Fake patrol starting point
                if (waypoints.Length > 0)
                    transform.position = waypoints[Random.Range(0, waypoints.Length)].position;

                Destroy(gameObject, cloneLifetime);
            }
        }

        protected override void Update()
        {
            if (player == null && !isClone) return;

            if (isClone)
            {
                // Fake patrol only
                PatrolOrMove();
                return;
            }

            // Real one: detect player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
            }
            else
            {
                PatrolOrMove();
            }

            if (distanceToPlayer <= attackRange)
            {
                TryAttack();
            }
        }

        protected override void PatrolOrMove()
        {
            if (waypoints.Length == 0 || isWaiting) return;

            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, target.position) < 0.2f)
            {
                StartCoroutine(PauseAtWaypoint());
            }
        }

        private IEnumerator PauseAtWaypoint()
        {
            isWaiting = true;
            yield return new WaitForSeconds(waypointPause);
            currentIndex = (currentIndex + 1) % waypoints.Length;
            isWaiting = false;
        }

        public void SpawnClones()
        {
            for (int i = 0; i < cloneCount; i++)
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * 3f;
                spawnPos.y = transform.position.y;

                GameObject cloneObj = Instantiate(clonePrefab, spawnPos, Quaternion.identity);

                if (cloneObj.TryGetComponent<MirrorCat>(out var cloneScript))
                {
                    cloneScript.isClone = true;
                    cloneScript.waypoints = this.waypoints; // give clones same path
                }
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (isClone)
            {
                if (canBeDestroyed && other.CompareTag("PlayerAttack"))
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (other.CompareTag("Player") && canAttack)
                {
                    if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                    {
                        playerHealth.TakeDamage(attackDamage);
                        StartCoroutine(AttackCooldown());
                    }
                }
            }
        }

        private void TryAttack()
        {
            if (canAttack && player != null)
            {
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
                    {
                        playerHealth.TakeDamage(attackDamage);
                        StartCoroutine(AttackCooldown());
                    }
                }
            }
        }

        private IEnumerator AttackCooldown()
        {
            canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }

        private void ChasePlayer()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }
}
