using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Platforms.FallingPlatform
{
    public class FallingPlatform : MonoBehaviour
    {
        public float fallDelay = 1f; // Delay before the platform falls
        public float resetDelay = 5f; // Time before platform resets

        private Rigidbody rb;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Invoke(nameof(DropPlatform), fallDelay);
            }
        }

        void DropPlatform()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            StartCoroutine(ResetPlatformAfterDelay());
        }

        IEnumerator ResetPlatformAfterDelay()
        {
            yield return new WaitForSeconds(resetDelay);

            rb.isKinematic = true;
            rb.useGravity = false;
            transform.SetPositionAndRotation(originalPosition, originalRotation);
        }
    }
}