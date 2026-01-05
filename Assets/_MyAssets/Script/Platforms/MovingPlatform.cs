using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.Platforms.MovingPlatform
{
    public class MovingPlatform : MonoBehaviour
    {
        public Transform pointA; // First position
        public Transform pointB; // Second position
        public float speed = 2f;
        public float waitTime = 5f; // Time to wait at each point

        private Transform target;
        private bool isWaiting = false;

        // Start is called before the first frame update
        void Start()
        {
            target = pointB; // Start moving towards point B
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isWaiting)
            {
                MovePlatform();
            }
        }

        void MovePlatform()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                StartCoroutine(WaitAndSwitchTarget());
            }
        }

        IEnumerator WaitAndSwitchTarget()
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            target = target == pointA ? pointB : pointA; // Switch target when reaching a point
            isWaiting = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Check if it's the player
            {
                other.transform.parent = transform; // Attach player to platform
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = null; // Detach player when they leave
            }
        }
    }
}