using UnityEngine;
using UnityEngine.AI;
using MR.Game.Player;

namespace MR.Game.Enemy
{
    public class BossEnemy : Enemy
    {
        [Header("Boss AI")]
        public float chaseRange = 15f;
        public float attackRange = 3f;
        public float attackCooldown = 2f;

        [Header("Roaming")]
        public float roamRadius = 8f;
        public float roamWaitTime = 3f;

        [Header("References")]
        [SerializeField] private Transform player;

        NavMeshAgent agent;
        BossAnimator bossAnimator;

        Vector3 spawnPoint;
        float roamTimer;
        bool canAttack = true;
        bool isDead = false;

        // -------------------- AWAKE --------------------
        protected override void Awake()
        {
            base.Awake();

            agent = GetComponent<NavMeshAgent>();
            bossAnimator = GetComponent<BossAnimator>();

            spawnPoint = transform.position;
            roamTimer = roamWaitTime;

            agent.isStopped = false;
        }

        // -------------------- MAIN LOGIC --------------------
        protected override void PatrolOrMove()
        {
            if (isDead || !player) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange && canAttack)
            {
                Attack();
            }
            else if (distance <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Roam();
            }

            // Drive Idle / Run animation
            bossAnimator.SetSpeed(agent.velocity.magnitude);
        }

        // -------------------- MOVEMENT --------------------
        void ChasePlayer()
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        void Roam()
        {
            roamTimer += Time.deltaTime;

            if (roamTimer >= roamWaitTime)
            {
                Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * roamRadius;

                if (NavMesh.SamplePosition(
                    randomPoint,
                    out NavMeshHit hit,
                    roamRadius,
                    NavMesh.AllAreas))
                {
                    agent.isStopped = false;
                    agent.SetDestination(hit.position);
                }

                roamTimer = 0f;
            }
        }

        // -------------------- COMBAT --------------------
        void Attack()
        {
            canAttack = false;
            agent.isStopped = true;

            if (Random.value < 0.35f)
                bossAnimator.HeavyAttack();
            else
                bossAnimator.LightAttack();

            Invoke(nameof(ResetAttack), attackCooldown);
        }

        void ResetAttack()
        {
            canAttack = true;
        }

        // -------------------- DAMAGE --------------------
        public override void TakeDamage(int dmg)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                bossAnimator.Hit(); // Boss does NOT get stunned
            }
        }

        protected override void Die()
        {
            if (isDead) return;

            isDead = true;
            bossAnimator.Die();
            agent.isStopped = true;

            enabled = false;
            Destroy(gameObject, 5f);
        }

        // -------------------- STUN OVERRIDE --------------------
        public override System.Collections.IEnumerator Stun(float duration)
        {
            // Boss ignores stun completely
            yield break;
        }

        // -------------------- DEBUG GIZMOS --------------------
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                spawnPoint == Vector3.zero ? transform.position : spawnPoint,
                roamRadius
            );

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
