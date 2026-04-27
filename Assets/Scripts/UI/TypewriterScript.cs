using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterScript : MonoBehaviour
{

    [SerializeField] private string testText;

    private TMP_Text textBox;
    private int currentVisibleCharacterIndex;
    private Coroutine typewriterCoroutine;
    private WaitForSeconds simpleDelay;
    private WaitForSeconds interpunctuationDelay;

    [SerializeField] private float characterPerSecond = 12;
    [SerializeField] private float floatinterpunctuationDelay = 0.5f;

    private void Awake()
    {
        textBox = GetComponent<TMP_Text>();

        simpleDelay = new WaitForSeconds(1 / characterPerSecond);
        interpunctuationDelay = new WaitForSeconds(floatinterpunctuationDelay);
    }

    private void Start()
    {
        SetText(testText);
    }

    public void SetText(string Text)
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        textBox.text = Text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typewriterCoroutine = StartCoroutine(routine: Typewriter());
    }

    public void SetTEventText(int charPerSecond, string Text)
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        textBox.text = Text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typewriterCoroutine = StartCoroutine(routine: EventTypewriter(charPerSecond));
    }


    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = textBox.textInfo;

        while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            textBox.maxVisibleCharacters++;

            if (character == '?' || character == '!' || character == ',' || character == '.' || character == ';' || character == ':' || character == '-')
            {
                yield return interpunctuationDelay;
            }
            else
            {
                yield return simpleDelay;
            }

            currentVisibleCharacterIndex++;

        }
    }

    private IEnumerator EventTypewriter(int charPerSecondMod)
    {
        TMP_TextInfo textInfo = textBox.textInfo;

        while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            textBox.maxVisibleCharacters++;

            if (character == '?' || character == '!' || character == ',' || character == '.' || character == ';' || character == ':' || character == '-')
            {
                yield return interpunctuationDelay;
            }
            else
            {
                if (charPerSecondMod > 0)
                {
                    simpleDelay = new WaitForSeconds(1 / characterPerSecond);
                }
                else
                {
                    simpleDelay = new WaitForSeconds(1 / charPerSecondMod);
                }
                    yield return simpleDelay;
            }

            currentVisibleCharacterIndex++;

        }
    }
}
