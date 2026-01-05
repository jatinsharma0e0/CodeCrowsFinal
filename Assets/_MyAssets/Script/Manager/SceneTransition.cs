using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace MR.Game.UI
{
    /// <summary>
    /// Manages scene transitions with a fade effect.
    /// </summary>
    /// <remarks>This class provides functionality to transition between scenes using a fade-in and fade-out
    /// effect. It ensures that the transition manager persists across scenes and can be accessed globally via the <see
    /// cref="Instance"/> property. The fade effect is controlled by the <see cref="fadeImage"/> and <see
    /// cref="fadeDuration"/> fields.</remarks>
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance { get; private set; }
        public Image fadeImage;
        public float fadeDuration = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FadeToScene(string sceneName)
        {
            StartCoroutine(FadeOutIn(sceneName));
        }

        private IEnumerator FadeOutIn(string sceneName)
        {
            // Fade to black
            yield return StartCoroutine(Fade(1f));
            // Load scene
            SceneManager.LoadScene(sceneName);
            // Optionally wait for scene to load
            yield return null;
            // Fade from black
            yield return StartCoroutine(Fade(0f));
        }

        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = fadeImage.color.a;
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, targetAlpha);
        }
    }
}
