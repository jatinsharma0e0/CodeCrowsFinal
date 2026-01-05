using MR.Game.Player;
using System.Collections;
using UnityEngine;

namespace MR.Game.Traps
{
    public class RollerTrap : Trap
    {
        public Transform pointA; // Start position
        public Transform pointB; // End position
        public float speed = 3f; // Movement speed
        public float waitTime = 2f; // Wait at each end
        public float rollSpeed = 360f; // Degrees per second for rolling
        public float radius = 0.5f; // Roller radius for realistic rotation

        private Transform target;
        private bool isWaiting = false;

        private void Start()
        {
            target = pointB;
        }

        private void FixedUpdate()
        {
            if (!isWaiting)
            {
                MoveAndRoll();
            }
        }

        private void MoveAndRoll()
        {
            // Move between points
            //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

            // Roll (rotate around local X axis)
            //transform.Rotate(Vector3.up, rollSpeed * Time.fixedDeltaTime, Space.Self);
            Vector3 prevPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            Vector3 movement = transform.position - prevPosition;

            // Only rotate if moved
            if (movement.sqrMagnitude > 0.0001f)
            {
                // Calculate rotation axis (perpendicular to movement and up)
                Vector3 rotationAxis = Vector3.Cross(movement.normalized, Vector3.down);
                // Calculate rotation angle based on arc length = radius * angle => angle = distance / radius
                float distance = movement.magnitude;
                float angle = Mathf.Rad2Deg * (distance / Mathf.Max(radius, 0.01f));
                transform.Rotate(rotationAxis, angle, Space.World);
            }

            // Switch target when reached
            if (Vector3.Distance(transform.position, target.position) < 0.05f)
            {
                StartCoroutine(WaitAndSwitchTarget());
            }
        }

        private IEnumerator WaitAndSwitchTarget()
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            target = target == pointA ? pointB : pointA;
            isWaiting = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Activate(other.gameObject);
                if (other.TryGetComponent<PlayerHealth>(out var health))
                {
                    health.TakeDamage(damage);
                }
            }
        }

        public override void Activate(GameObject target)
        {
            base.Activate(target);
            Debug.Log("RollerTrap: Player hit by rolling trap!");
            // Add damage or respawn logic here
        }
    }
}
