using UnityEngine;

namespace MR.Game.Enemy
{
    public class YarnBallProjectile : MonoBehaviour
    {
        public int damage = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<MR.Game.Player.PlayerHealth>(out var health))
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Enemy")) // Don't collide with other enemies
            {
                Destroy(gameObject);
            }
        }
    }
}
