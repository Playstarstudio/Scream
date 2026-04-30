using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaNameScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string areaName = SceneManager.GetActiveScene().ToString();

        TypewriterScript typewriterScript = transform.GetComponent<TypewriterScript>();
        typewriterScript.SetText(areaName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
