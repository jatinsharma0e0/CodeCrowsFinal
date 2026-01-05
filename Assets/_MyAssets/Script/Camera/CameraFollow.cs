using MR.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Game.CameraFollow
{
    /// <summary>
    /// Controls the camera to follow the player with smooth movement and dynamic offset adjustments.
    /// </summary>
    /// <remarks>The camera follows the player's position with a configurable offset and smooth speed.  If the
    /// player is moving backward, the camera adjusts its offset to provide a different perspective. This script
    /// requires the player GameObject to have a <see cref="PlayerMovement"/> component.</remarks>
    public class CameraFollow : MonoBehaviour
    {
        public Transform player; // Assign the player in the Inspector
        public float smoothSpeed = 5f; // Speed of camera movement
        public Vector3 offset = new(0, 3, -7); // Camera position offset
        public PlayerMovement playerMovement; // Assign in Inspector
        public Vector3 backwardOffset = new(0, 3, 7); // Camera offset when moving backward
        public Vector3 leftOffset = new(-2, 3, 7); // Camera offset when moving backward
        public Vector3 rightOffset = new(2, 3, 7); // Camera offset when moving backward

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            player = GameObject.FindWithTag("Player").transform;
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
        private void FixedUpdate()
        {
            /*if (player != null)
            {
                Vector3 targetPosition = player.position + offset; // Calculate target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                transform.LookAt(player); // Keeps the camera looking at the player
            }*/

            if (player != null && playerMovement != null)
            {
                // Check if player is moving backward (moveInput.y < -0.1f)
                Vector3 usedOffset = offset;
                if (playerMovement.CurrentMoveInput.y < -0.1f)
                {
                    usedOffset = backwardOffset;
                }

                if (playerMovement.CurrentMoveInput.x < -0.1f)
                {
                    usedOffset = leftOffset;
                }
                else if (playerMovement.CurrentMoveInput.x > 0.1f)
                {
                    usedOffset = rightOffset;
                }

                Vector3 targetPosition = player.position + usedOffset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                //transform.LookAt(player);
            }
        }

    }
}