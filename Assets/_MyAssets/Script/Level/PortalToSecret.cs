using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR.Game.Level.Portals
{
    /// <summary>
    /// This script handles entering a secret level through a portal.
    /// </summary>
    public class PortalToSecret : MonoBehaviour
    {
        public string secretLevelSceneName = "SecretLevel"; // Must match scene name exactly
        public float delayBeforeLoad = 0.5f;

        private bool playerEntered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!playerEntered && other.CompareTag("Player"))
            {
                playerEntered = true;
                Debug.Log("🔮 Entering secret portal!");
                Invoke(nameof(LoadSecretLevel), delayBeforeLoad);
            }
        }

        private void LoadSecretLevel()
        {
            SceneManager.LoadScene(secretLevelSceneName);
        }
    }
}
