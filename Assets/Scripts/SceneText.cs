using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneText : MonoBehaviour
{
    // idgaf im hardcoding this shit i dont wanna re-enter SFs over and over
    public string defaultSceneText = "";


    public string frontCabinSceneText = "Another report of paranormal spectacle in a place of disrepair.";


    public string bedroomChangeSceneText = "Peculiar. This room was... different.";


    public string tentRoomSpawnSceneText = "There was naught but a wall there a minute past...";

    
    public string tentRoomIntSceneText = "What the fuck.";

    
    public string kitchenLayout3SceneText = "My brain pounds against my skull...";


    public string frontDoorGoneSceneText = "Something lurks after all. The door--it vanished the front door.";

    private void Awake()
    {
        SceneManager.sceneLoaded += DoSceneText;
    }

    private void DoSceneText(Scene scene, LoadSceneMode mode)
    {
        string textToShow = defaultSceneText;

        // Check which layout is active
        if (GameObject.Find("FrontCabin") != null && GameObject.Find("FrontCabin").activeInHierarchy)
        {
            textToShow = frontCabinSceneText;
        }
        else if (GameObject.Find("TentRoomSpawn") != null && GameObject.Find("TentRoomSpawn").activeInHierarchy)
        {
            textToShow = tentRoomSpawnSceneText;
        }
        else if (GameObject.Find("TentRoomInt") != null && GameObject.Find("TentRoomInt").activeInHierarchy)
        {
            textToShow = tentRoomIntSceneText;
        }
        else if (GameObject.Find("KitchenLayout3") != null && GameObject.Find("KitchenLayout3").activeInHierarchy)
        {
            textToShow = kitchenLayout3SceneText;
        }
        else if (GameObject.Find("BedroomLayout2") != null && GameObject.Find("BedroomLayout2").activeInHierarchy)
        {
            textToShow = bedroomChangeSceneText;
        }
        else if (GameObject.Find("FoyerNoFront") != null && GameObject.Find("FoyerNoFront").activeInHierarchy)
        {
            textToShow = frontDoorGoneSceneText;
        }

        GameObject.Find("BillboardText").GetComponent<TypewriterScript>().SetText(textToShow);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DoSceneText;
    }

}
