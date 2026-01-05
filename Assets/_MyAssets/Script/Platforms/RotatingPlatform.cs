using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Platforms.RotatingPlatform
{
    public class RotatingPlatform : MonoBehaviour
    {
        public float rotationSpeed = 50f; // Speed of rotation
        public float colorChangeInterval = 2f; // Time between color changes
        private Renderer platformRenderer;
        private Color targetColor;
        private float timer;

        // Start is called before the first frame update
        void Start()
        {
            platformRenderer = GetComponent<Renderer>();
            ChangeColor(); // Set an initial color
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Rotate the platform
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);

            // Change color instantly at intervals
            timer += Time.deltaTime;
            if (timer >= colorChangeInterval)
            {
                ChangeColor();
                timer = 0f; // Reset timer
            }
        }

        void ChangeColor()
        {
            targetColor = Random.ColorHSV(); // Pick a new random color
            platformRenderer.material.color = targetColor; // Apply new color instantly
        }

        // Make player a child of the platform when on it
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = transform; // Attach player to platform
            }
        }

        // Unparent player when leaving the platform
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.SetParent(null);
            }
        }
    }
}