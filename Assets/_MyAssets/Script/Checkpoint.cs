using MR.Game.Player;
using UnityEngine;

namespace MR.Game.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        private bool isActive = false;

        [Header("Feedback Settings")]
        public Renderer checkpointRenderer;
        public Renderer checkpointRenderer1;
        public Color activeColor = Color.green;
        public ParticleSystem activateParticles;
        public AudioSource activateSound;

        public Color originalColor;

        private void Start()
        {
            if (checkpointRenderer != null)
            {
                originalColor = checkpointRenderer.material.color;
            }
            if (checkpointRenderer1 != null)
            {
                originalColor = checkpointRenderer1.material.color;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isActive)
            {
                CheckpointManager.Instance.SetActiveCheckpoint(this);
                ActivateCheckpoint();
                Debug.Log("Checkpoint reached: " + gameObject.name);
            }
        }

        public void ActivateCheckpoint()
        {
            isActive = true;

            if (checkpointRenderer != null)
                checkpointRenderer.material.color = activeColor;

            if (checkpointRenderer1 != null)
                checkpointRenderer1.material.color = activeColor;

            if (activateParticles != null)
                activateParticles.Play();

            if (activateSound != null)
                activateSound.Play();
        }

        public void DeactivateCheckpoint()
        {
            isActive = false;

            if (checkpointRenderer != null)
                checkpointRenderer.material.color = originalColor;

            if (checkpointRenderer1 != null)
                checkpointRenderer1.material.color = originalColor;
        }
    }
}