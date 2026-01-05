using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MR.Game.Player; 

namespace MR.Game.Manager
{
    // This class is intended to manage the game state, score, and player lives.
    // It will be a singleton to ensure only one instance exists throughout the game.
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int score = 0;
        private PlayerHealth playerHealth;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            // Find the PlayerHealth component in the scene
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }
        }

        public void AddScore(int amount)
        {
            score += amount;
            // Update UI here if needed
        }

        public void PlayerDied()
        {
            if (playerHealth == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                    playerHealth = playerObj.GetComponent<PlayerHealth>();
            }

            if (playerHealth != null && playerHealth.currentHealth <= 0)
            {
                GameOver();
            }
            else
            {
                // Respawn player or reload checkpoint
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over!");
            // Show game over UI, restart level, etc.

            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        internal void LevelComplete()
        {
            Debug.Log("Level Complete!");
            Debug.Log("Next level unlocked");
        }
    }
}