using UnityEngine;

namespace MR.Game.Enemy
{
    public class PuppetCrow : Enemy
    {
        [Header("Ranged Attack Settings")]
        public GameObject projectilePrefab;
        public Transform firePoint;
        public float detectionRange = 10f;
        public float fireRate = 2f;
        private float fireCooldown;

        private Transform player;

        private void Start() => player = GameObject.FindGameObjectWithTag("Player")?.transform;

        protected override void PatrolOrMove()
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                LookAtPlayer();

                if (Time.time > fireCooldown)
                {
                    Shoot();
                    fireCooldown = Time.time + fireRate;
                }
            }
        }

        private void LookAtPlayer()
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0; // keep rotation flat
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }
        }

        private void Shoot()
        {
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                if (proj.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.linearVelocity = firePoint.forward * 10f; // projectile speed
                }
            }
        }
    }
}
