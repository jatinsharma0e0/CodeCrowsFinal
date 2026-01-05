using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public class DreamBat : Enemy
    {
        private enum BatState
        {
            Idle,
            Swooping,
            Returning,
            Sleeping
        }

        [Header("Bat Settings")]
        public Transform perchPoint;
        public float detectionRange = 6f;
        public float swoopSpeed = 6f;
        public float returnSpeed = 4f;
        public float attackRange = 1f;
        public float sleepTime = 1.5f;
        public bool ignoreHeight = true;

        [Header("Bobbing")]
        public float bobAmplitude = 0.3f;
        public float bobFrequency = 2f;

        private Transform player;
        private Vector3 perchPos;
        private float bobOffset;

        private BatState state = BatState.Idle;

        private bool hasDealtDamage = false;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (perchPoint != null)
            {
                transform.position = perchPoint.position;
                perchPos = perchPoint.position;
                bobOffset = Random.Range(0f, 10f);      // prevents synchronized bobbing
            }
        }

        protected override void PatrolOrMove()
        {
            switch (state)
            {
                case BatState.Idle:
                    Bob();
                    TryStartSwoop();
                    break;

                case BatState.Swooping:
                    SwoopMovement();
                    break;

                case BatState.Returning:
                    ReturnToPerch();
                    break;

                case BatState.Sleeping:
                    Bob();
                    break;
            }
        }

        // ---------------------------------------------------------
        // ⭐ STATE LOGIC
        // ---------------------------------------------------------

        private void TryStartSwoop()
        {
            if (player == null) return;

            if (IsPlayerInSight())
            {
                state = BatState.Swooping;
                hasDealtDamage = false;
            }
        }

        private void SwoopMovement()
        {
            if (player == null)
            {
                state = BatState.Returning;
                return;
            }

            Vector3 target = player.position;

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                swoopSpeed * Time.deltaTime
            );

            RotateTowards(target - transform.position);

            // reached attack zone → return
            if (Vector3.Distance(transform.position, target) <= attackRange)
            {
                state = BatState.Returning;
            }
        }


        private void ReturnToPerch()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                perchPos,
                returnSpeed * Time.deltaTime
            );

            RotateTowards(perchPos - transform.position);

            if (Vector3.Distance(transform.position, perchPos) < 0.1f)
                StartCoroutine(SleepRoutine());
        }


        private IEnumerator SleepRoutine()
        {
            state = BatState.Sleeping;
            yield return new WaitForSeconds(sleepTime);
            state = BatState.Idle;
        }

        // ---------------------------------------------------------
        // ⭐ BOBBING
        // ---------------------------------------------------------
        private void Bob()
        {
            float newY = perchPos.y + Mathf.Sin((Time.time + bobOffset) * bobFrequency) * bobAmplitude;
            transform.position = new Vector3(perchPos.x, newY, perchPos.z);
        }

        // ---------------------------------------------------------
        // ⭐ DETECTION
        // ---------------------------------------------------------
        private bool IsPlayerInSight()
        {
            float dist = GetFlatDistance(transform.position, player.position);
            if (dist > detectionRange) return false;

            // Raycast check (optional)
            Vector3 dir = (player.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, detectionRange))
            {
                return hit.collider.CompareTag("Player");
            }

            return false;
        }

        private float GetFlatDistance(Vector3 a, Vector3 b)
        {
            if (!ignoreHeight) return Vector3.Distance(a, b);

            a.y = 0;
            b.y = 0;
            return Vector3.Distance(a, b);
        }

        private void RotateTowards(Vector3 dir)
        {
            if (dir.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (dir.x < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // ---------------------------------------------------------
        // ⭐ DAMAGE ON IMPACT
        // ---------------------------------------------------------
        protected override void OnTriggerEnter(Collider other)
        {
            if (state != BatState.Swooping) return;
            if (hasDealtDamage) return;

            if (other.CompareTag("Player") &&
                other.TryGetComponent<MR.Game.Player.PlayerHealth>(out var hp))
            {
                hp.TakeDamage(contactDamage);
                hasDealtDamage = true;
                Debug.Log($"{name} hit the player!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Vector3 pos = perchPoint != null ? perchPoint.position : transform.position;
            Gizmos.DrawWireSphere(pos, detectionRange);
        }
    }
}
