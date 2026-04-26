using UnityEngine;

public class CanvasStartScript : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        Camera mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }


}
