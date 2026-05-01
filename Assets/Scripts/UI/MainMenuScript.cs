using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour //, IPointerEnterHandler
{
    private AudioManager _audio;
    
    void Awake()
    {
        _audio = AudioManager.Instance;
    }
    
    public void StartButton()
    {
        SceneManager.LoadScene("Outside");
        _audio.PlayOneShot(AudioID.SFX.Interface.Settings.confirm);
    }

    public void ExitButton()
    {
        _audio.PlayOneShot(AudioID.SFX.Interface.Settings.deny);
        Application.Quit();
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     _audio.PlayOneShot(AudioID.SFX.Interface.Inventory.hover);
    // }
}
