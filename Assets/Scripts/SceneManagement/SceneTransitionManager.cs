using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    // Called by Door. Records the current scene as the origin, then loads the target scene.
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
            if (door.TargetSceneName == _originSceneName)
            {
                TeleportPlayer(door.SpawnPosition, door.FacingDirection);
                break;
            }
        }

        // Clear so we don't re-teleport if the scene is reloaded
        _originSceneName = null;
    }

    private static void TeleportPlayer(Vector3 position, Vector2 facingDirection)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = position;

            var movement = player.GetComponent<CharacterMovement>();
            if (movement != null)
            {
                movement.SetFacingDirection(facingDirection);
            }
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
