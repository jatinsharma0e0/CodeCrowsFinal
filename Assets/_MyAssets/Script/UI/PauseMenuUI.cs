using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MR.Game
{
    public class PauseMenuUI : MonoBehaviour
    {
        public Button resumeButton;
        public Button optionButton;
        public Button quitButton;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            resumeButton.onClick.AddListener(ResumeGame);
            optionButton.onClick.AddListener(Option);
            quitButton.onClick.AddListener(OnApplicationQuit);
        }

        public void ResumeGame()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void Option()
        {

        }

        public void OnApplicationQuit()
        {
            SceneManager.LoadScene(0);
        }
    }
}
