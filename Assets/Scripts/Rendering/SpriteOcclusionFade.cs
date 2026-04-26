using System.Collections;
using UnityEngine;

public class SpriteOcclusionFade : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Tag used to identify the player character.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("If assigned, only these renderers will fade. Otherwise all SpriteRenderers on this object and its children are used.")]
    [SerializeField] private SpriteRenderer[] targetRenderers;

    [Header("Fade Settings")]
    [Tooltip("The alpha the sprite fades to when the player is behind it (0 = fully invisible).")]
    [SerializeField, Range(0f, 1f)] private float fadedAlpha = 0.35f;

    [Tooltip("How quickly the sprite fades out (seconds).")]
    [SerializeField, Range(0.01f, 2f)] private float fadeOutSpeed = 2f;

    [Tooltip("How quickly the sprite fades back in (seconds).")]
    [SerializeField, Range(0.01f, 2f)] private float fadeInSpeed = 0.3f;

    private float _currentAlpha = 1f;
    private bool _playerBehind;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            targetRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        _currentAlpha = 1f;
        _playerBehind = false;
        ApplyAlpha(_currentAlpha);
    }

    private void OnDisable()
    {
        _fadeCoroutine = null;
    }

    private void StartFadeIfNeeded()
    {
        if (_fadeCoroutine == null && isActiveAndEnabled)
            _fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        while (true)
        {
            float targetAlpha = _playerBehind ? fadedAlpha : 1f;
            float speed = _playerBehind ? fadeOutSpeed : fadeInSpeed;

            _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, Time.deltaTime / speed);
            ApplyAlpha(_currentAlpha);

            if (Mathf.Approximately(_currentAlpha, targetAlpha))
            {
                _fadeCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }

    private void ApplyAlpha(float alpha)
    {
        foreach (var spriteRenderer in targetRenderers)
        {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = alpha;
            spriteRenderer.color = spriteColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerBehind = true;
            StartFadeIfNeeded();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerBehind = false;
            StartFadeIfNeeded();
        }
    }
}
