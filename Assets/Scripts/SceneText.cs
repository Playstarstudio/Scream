using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneText : MonoBehaviour
{
    [SerializeField]
    public string sceneText;

    private void Awake()
    {
        SceneManager.sceneLoaded += DoSceneText;
    }

    private void DoSceneText(Scene scene, LoadSceneMode mode)
    {

        GameObject.Find("BillboardText").GetComponent<TypewriterScript>().SetText(sceneText);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DoSceneText;
    }

}
