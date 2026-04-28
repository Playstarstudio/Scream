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
        textBox.text = Text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;
        
        textBox.ForceMeshUpdate();
        
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        typewriterCoroutine = StartCoroutine(routine: Typewriter());
    }

    public void SetEventText(int charPerSecond, string Text)
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
        textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = textBox.textInfo;
        textBox.maxVisibleCharacters++;
        foreach (var charText in textInfo.characterInfo)
        //while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {
            textBox.maxVisibleCharacters++;
            //Debug.Log(currentVisibleCharacterIndex + " " + textInfo.characterCount);
            char character = charText.character;
            //char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;


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
        textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = textBox.textInfo;
        textBox.maxVisibleCharacters++;
        foreach (var charText in textInfo.characterInfo)
        {
            textBox.maxVisibleCharacters++;
            //while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {

                char character = charText.character;
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

    bool IsValidIndex(int index, TMP_TextInfo textInfo)
    {
        if (index < 0 || index >= textInfo.characterInfo.Length)
        {
            Debug.Log("1");
            return false;

        }
        if (index >= textInfo.characterCount)
        {
            Debug.Log("2");

            return false;
        }
        Debug.Log("3");

        return textInfo.characterInfo[index].character != 0;
    }
}
