using UnityEngine;

namespace MR.Game.Enemy
{
    public class BossAnimator : MonoBehaviour
    {
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetSpeed(float speed)
        {
            animator.SetFloat("Speed", speed);
        }

        public void LightAttack()
        {
            animator.SetTrigger("LightAttack");
        }

        public void HeavyAttack()
        {
            animator.SetTrigger("HeavyAttack");
        }

        public void Hit()
        {
            animator.SetTrigger("Hit");
        }

        public void Die()
        {
            animator.SetBool("IsDead", true);
        }
    }
}
