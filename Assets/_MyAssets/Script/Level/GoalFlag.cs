using MR.Game.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR.Game.Level
{
    // GoalFlag is a placeholder for a goal or checkpoint in the game level.
    // It can be used to trigger events when the player reaches this point.
    // This class currently does not implement any specific functionality.
    public class GoalFlag : MonoBehaviour
    {
        [Header("Scene to Load")]
        public string sceneToLoad; // Set this in the Inspector
        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Goal reached! Level complete.");
                GameManager.Instance.LevelComplete(); // Call your level complete logic
                // Optionally: play sound, show UI, etc.

                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
                else
                {
                    Debug.LogWarning("No scene specified to load!");
                }
            }
        }
    }
}