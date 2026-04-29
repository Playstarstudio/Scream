using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneText : MonoBehaviour
{
    [SerializeField]
    public string defaultSceneText;

    [SerializeField]
    public string frontCabinSceneText;

    [SerializeField]
    public string tentRoomSpawnSceneText;

    [SerializeField]
    public string tentRoomIntSceneText;

    [SerializeField]
    public string kitchenLayout3SceneText;

    private void Awake()
    {
        SceneManager.sceneLoaded += DoSceneText;
    }

    private void DoSceneText(Scene scene, LoadSceneMode mode)
    {
        string textToShow = defaultSceneText;

        // Check which layout GameObject is active
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

        GameObject.Find("BillboardText").GetComponent<TypewriterScript>().SetText(textToShow);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DoSceneText;
    }

}
