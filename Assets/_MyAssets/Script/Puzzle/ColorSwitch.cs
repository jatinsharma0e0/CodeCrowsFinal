using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Puzzle
{
    [System.Serializable]
    public class TargetEffect
    {
        public ColorTarget target;
        public bool activate; // true = open/enable, false = close/disable
    }
    public class ColorSwitch : MonoBehaviour
    {
        public List<TargetEffect> effects;
        public Color color = Color.white; // Assign in Inspector
        public Renderer targetRenderer;   // Assign in Inspector

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            targetRenderer = GetComponent<Renderer>();

            // Set the material color
            if (targetRenderer != null)
            {
                // Make sure we're not editing a shared material unless that's intended
                targetRenderer.material = new Material(targetRenderer.material)
                {
                    color = color
                };
            }
            else
            {
                Debug.LogWarning("ColorTarget: targetRenderer not assigned!", this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ActivateSwitch();
            }
        }

        private void ActivateSwitch()
        {
            foreach (var effect in effects)
            {
                if (effect.activate)
                {
                    effect.target.OpenTarget();
                }
                else
                {
                    effect.target.CloseTarget();
                }
            }
        }
    }
}
