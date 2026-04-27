using System.Collections;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [Tooltip("Text box for text to appear in")]
    [SerializeField]
    private TMP_Text _textBox;

    [Tooltip("Fade in speed")]
    [SerializeField]
    private float _fadeInSpeed = 1;

    [Tooltip("Fade out speed")]
    [SerializeField]
    private float _fadeOutSpeed = 1;

    private float _visibleAlpha =  1f;
    private float _invisibleAlpha = 0f;
    private float _targetAlpha = 0f;
    bool _fading = false;

    void Start()
    {
        _fading = false;
        _textBox.alpha = _invisibleAlpha;
    }

    IEnumerator FadeCoroutine()
    {
        _fading = true;
        while (!Mathf.Approximately(_textBox.alpha, _targetAlpha))
        {
            float fadeSpeed = _targetAlpha == _visibleAlpha ? _fadeInSpeed : _fadeOutSpeed;
           _textBox.alpha = Mathf.MoveTowards(_textBox.alpha, _targetAlpha, fadeSpeed * Time.deltaTime);
            yield return new WaitForSeconds(.001f);
        }

        _fading = false;

    } 

    public void FadeInText()
    {
        if (!_fading) StartCoroutine(FadeCoroutine()); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Player"))
        {
            _targetAlpha = _visibleAlpha;
            FadeInText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Player"))
        {
            _targetAlpha = _invisibleAlpha;
            FadeInText();
        }
    }
}
