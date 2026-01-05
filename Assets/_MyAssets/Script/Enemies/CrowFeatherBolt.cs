using UnityEngine;

namespace MR.Game.Enemy
{
    public class CrowFeatherBolt : MonoBehaviour
    {
        public int damage = 1;
        public float lifetime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MR.Game.Player.PlayerHealth health = other.GetComponent<MR.Game.Player.PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Enemy")) // don't hit other enemies
            {
                Destroy(gameObject);
            }
        }
    }
}
