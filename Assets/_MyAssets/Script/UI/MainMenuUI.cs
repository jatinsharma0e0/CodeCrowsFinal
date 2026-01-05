using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MR.Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        public Button startButton;
        public Button quitButton;

        [Header("Scene Settings")]
        [SerializeField] private string gameSceneName = "GameScene";
        void Start()
        {
            startButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
        }


        public void PlayGame()
        {
            SceneManager.LoadScene(1);

        }

        public void QuitGame()
        {
            Debug.Log("Game quit");
            Application.Quit();
        }
    }
}
