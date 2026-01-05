using UnityEngine;

namespace MR.Game.Enemy
{
    public class YarnBallProjectile1 : MonoBehaviour
    {
        public int damage = 1;
        public float lifetime = 6f;        // How long before auto-destroy
        public int maxBounces = 3;         // Max number of bounces allowed

        private int bounceCount = 0;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Damage player
            if (collision.collider.CompareTag("Player"))
            {
                MR.Game.Player.PlayerHealth health = collision.collider.GetComponent<MR.Game.Player.PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }

            // Count bounces
            bounceCount++;
            if (bounceCount > maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}
