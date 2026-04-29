using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{

    public class SceneTransitionManager : IService
    {
        private readonly float _fadeDuration;
        private readonly CanvasGroup _fadeCanvasGroup;

        /// The scene we just left. Used to find the arrival door in the new scene.
        private string _originSceneName;

        private bool _isTransitioning;

        public SceneTransitionManager(float fadeDuration = 0f, CanvasGroup fadeCanvasGroup = null)
        {
            _fadeDuration = fadeDuration;
            _fadeCanvasGroup = fadeCanvasGroup;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        public void TransitionToScene(string targetSceneName)
        {
            if (_isTransitioning) return;

            _originSceneName = SceneManager.GetActiveScene().name;

            if (_fadeDuration > 0f && _fadeCanvasGroup != null)
            {
                var mono = ServiceLocator.Instance.Get<MonoBehaviorService>();
                mono.StartCoroutine(TransitionCoroutine(targetSceneName));
            }
            else
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (string.IsNullOrEmpty(_originSceneName)) return;

            // Find the door that points back to the scene we came from
            Door[] doors = Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
            foreach (var door in doors)
            {
                if (door.enabled && door.TargetSceneName == _originSceneName)
                {
                    TeleportPlayer(door.SpawnPosition);
                    break;
                }
            }

            SceneText sceneText = GameObject.FindFirstObjectByType<SceneText>();
            if (sceneText != null)
            {
                GameObject.FindFirstObjectByType<TypewriterScript>().SetEventText(sceneText.charPerSecond, sceneText.sceneText);
            }

            _originSceneName = null;
        }

        private static void TeleportPlayer(Vector3 position)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = position;
            }
            else
            {
                Debug.LogWarning("[SceneTransitionManager] No GameObject with tag 'Player' found to teleport.");
            }
        }

        private IEnumerator TransitionCoroutine(string sceneName)
        {
            _isTransitioning = true;

            // Fade out
            yield return Fade(0f, 1f);

            SceneManager.LoadScene(sceneName);

            // Wait a frame for the new scene to initialise
            yield return null;

            // Fade in
            yield return Fade(1f, 0f);

            _isTransitioning = false;
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                _fadeCanvasGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / _fadeDuration));
                yield return null;
            }

            _fadeCanvasGroup.alpha = to;
        }
    }
}
