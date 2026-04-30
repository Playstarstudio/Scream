using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NarrativeZoomScript : MonoBehaviour
{
    public GameObject zoomContainer;
    public Image zoomImage;
    public bool zoomOpen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoomContainer = transform.Find("ZoomContainer").gameObject;
        CloseZoomCanvas();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && zoomOpen == true)
        {
            CloseZoomCanvas();
        }
    }


    public void OpenZoomCanvas(Sprite sprite)
    {
        zoomOpen = true;

        if (zoomContainer != null)
        {
            zoomContainer.SetActive(true);
        }
        if (zoomImage != null)
        {
            zoomImage.sprite = sprite;
            Time.timeScale = 0;
        }

    }

    public void CloseZoomCanvas()
    {
        zoomOpen = false;
        zoomContainer.SetActive(false);
        Time.timeScale = 1;
    }


}
