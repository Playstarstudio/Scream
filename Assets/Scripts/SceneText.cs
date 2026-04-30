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



    public string bedroomChangeFirstLine = "Peculiar. This room was... different.";
    public string bedroomChangeSecondLine = "What manner of being can do such a thing?";



    public string tentRoomSpawnFirstLine = "There was naught but a wall there a minute past...";
    public string tentRoomSpawnSecondLine = null;



    public string tentRoomIntFirstLine = "What the fuck.";
    public string tentRoomIntSecondLine = "This is no work of the demonic or paranormal... Worse. Far worse.";



    public string kitchenLayout3FirstLine = "My brain pounds against my skull...";
    public string kitchenLayout3SecondLine = null;



    public string frontDoorGoneFirstLine = "Something lurks after all. The door--it vanished the front door.";
    public string frontDoorGoneSecondLine = "Recreating the ritual will inform me of what I'm dealing with.";


    private void Start()
    {
        typewriterScript = GameObject.FindWithTag("Player").GetComponentInChildren<TypewriterScript>();

        DoSceneText();

        typewriterScript.firstText = firstLine;
        typewriterScript.secondText = secondLine;
        typewriterScript.SetText(typewriterScript.firstText);
    }

    private void DoSceneText()
    {

        //Check which layout is active
        if (GameObject.Find("FrontCabin") != null && GameObject.Find("FrontCabin").activeInHierarchy)
        {
            firstLine = frontCabinFirstLine;
            secondLine = frontCabinSecondLine;

        }
        else if (GameObject.Find("TentRoomSpawn") != null && GameObject.Find("TentRoomSpawn").activeInHierarchy)
        {
            firstLine = tentRoomSpawnFirstLine;
            secondLine = tentRoomSpawnSecondLine;

        }
        else if (GameObject.Find("TentRoomInt") != null && GameObject.Find("TentRoomInt").activeInHierarchy)
        {
            firstLine = tentRoomIntFirstLine;
            secondLine = tentRoomIntSecondLine;

        }
        else if (GameObject.Find("KitchenLayout3") != null && GameObject.Find("KitchenLayout3").activeInHierarchy)
        {
            firstLine = kitchenLayout3FirstLine;
            secondLine = kitchenLayout3SecondLine;

        }
        else if (GameObject.Find("BedroomLayout2") != null && GameObject.Find("BedroomLayout2").activeInHierarchy)
        {
            firstLine = bedroomChangeFirstLine;
            secondLine = bedroomChangeSecondLine;

        }
        else if (GameObject.Find("FoyerNoFront") != null && GameObject.Find("FoyerNoFront").activeInHierarchy)
        {
            firstLine = frontDoorGoneFirstLine;
            secondLine = frontDoorGoneSecondLine;

        }

    }

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= DoSceneText;
    //}

}
