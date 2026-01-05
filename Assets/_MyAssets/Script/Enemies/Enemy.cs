using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Enemy Base Stats")]
        public int maxHealth = 3;
        protected int currentHealth;

        public float moveSpeed = 2f;
        protected bool isStunned = false;

        [Header("Combat")]
        public int contactDamage = 1;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        protected virtual void Update()
        {
            if (isStunned) return;
            PatrolOrMove(); // child class implements its movement
        }

        // Movement behavior (abstract for children to define)
        protected abstract void PatrolOrMove();

        public virtual void TakeDamage(int dmg)
        {
            if (isStunned) return;

            currentHealth -= dmg;
            Debug.Log($"{gameObject.name} took {dmg} damage, HP = {currentHealth}");

            if (currentHealth <= 0)
                Die();
            else
                StartCoroutine(Stun(0.5f));
        }

        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} died!");
            Destroy(gameObject);
        }

        public virtual IEnumerator Stun(float duration)
        {
            isStunned = true;
            yield return new WaitForSeconds(duration);
            isStunned = false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamage(contactDamage);
                }
            }
        }
    }
}