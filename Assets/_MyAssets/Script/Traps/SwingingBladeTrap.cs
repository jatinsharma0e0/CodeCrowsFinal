using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Traps
{
    /// <summary>
    /// Represents a swinging blade trap that oscillates back and forth to damage or interact with players.
    /// </summary>
    /// <remarks>The trap swings at a specified angle and speed, creating a periodic motion. When a player
    /// enters the trap's trigger area, the trap activates and performs its defined behavior, such as dealing damage or
    /// triggering a response.</remarks>
    public class SwingingBladeTrap : Trap
    {
        public float swingAngle = 60f;      // Maximum swing angle (degrees)
        public float swingSpeed = 2f;       // Speed of swinging
        private Quaternion startRotation;
        private float timeOffset;

        private void Start()
        {
            startRotation = transform.localRotation;
            timeOffset = Random.Range(0f, Mathf.PI * 2f);// Optional: desync multiple blades
        }
        private void Update()
        {
            float angle = Mathf.Sin(Time.time * swingSpeed + timeOffset) * swingAngle;
            transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Activate(other.gameObject);
            }
        }

        public override void Activate(GameObject target)
        {
            base.Activate(target);
            Debug.Log("SwingingBladeTrap: Player hit by swinging blade!");
            // Add specific logic, e.g., damage player or respawn
            if (target.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}