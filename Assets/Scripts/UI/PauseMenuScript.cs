using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public bool gamePaused;
    public GameObject pauseContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseContainer = transform.Find("PauseContainer").gameObject;
        pauseContainer.SetActive(false);
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && gamePaused == false)
        {
            gamePaused = true;
            pauseContainer.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && gamePaused == true)
        {
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseContainer.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }


}
