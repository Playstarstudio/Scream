using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneText : MonoBehaviour
{
    [SerializeField] private string firstLine;
    [SerializeField] private string secondLine;

    public TypewriterScript typewriterScript;


    // idgaf im hardcoding this shit i dont wanna re-enter SFs over and over lmao
    public string defaultSceneText = "";


    public string frontCabinFirstLine = "Another report of paranormal spectacle in a place of disrepair.";
    public string frontCabinSecondLine = "I am most doubtful this investigation will have much of note.";


    public string tentRoomIntFirstLine = "What the fuck.";
    public string tentRoomIntSecondLine = "This is no work of the demonic or paranormal... Worse. Far worse.";


    private void Start()
    {

        typewriterScript = GameObject.FindWithTag("Player").GetComponentInChildren<TypewriterScript>();

        DoSceneText();

        TypewriterScript[] typewriterArray = GameObject.FindWithTag("Player").GetComponentsInChildren<TypewriterScript>();

        foreach (TypewriterScript typewriterScript in typewriterArray)
        {
            typewriterScript.firstText = firstLine;
            typewriterScript.secondText = secondLine;
            typewriterScript.SetText(typewriterScript.firstText);
        }
    }

    private void DoSceneText()
    {
        
        //Check which layout is active
        if (GameObject.Find("FrontCabin") != null && GameObject.Find("FrontCabin").activeInHierarchy)
        {
            firstLine = frontCabinFirstLine;
            secondLine = frontCabinSecondLine;

        }
        else if (GameObject.Find("TentRoomInt") != null && GameObject.Find("TentRoomInt").activeInHierarchy)
        {
            firstLine = tentRoomIntFirstLine;
            secondLine = tentRoomIntSecondLine;

        }
        
    }

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= DoSceneText;
    //}

}
