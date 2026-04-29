using UnityEngine;

public class HUDSingleton : MonoBehaviour
{
    public static HUDSingleton Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this, destroy this
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        // Optional: Keep this object alive across multiple scenes
        DontDestroyOnLoad(gameObject);
    }
}