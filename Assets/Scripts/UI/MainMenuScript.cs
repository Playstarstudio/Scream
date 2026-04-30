using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    public void StartButton()
    {
        SceneManager.LoadScene("Outside");
    }

    public void ExitButton()
    {
        Application.Quit();
    }


}
