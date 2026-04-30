using UnityEngine;
using UnityEngine.UI;

public class UnscaledTime : MonoBehaviour
{
    public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = transform.GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_UnscaledTime", material.GetFloat("_UnscaledTime") + Time.unscaledDeltaTime);
    }
}
