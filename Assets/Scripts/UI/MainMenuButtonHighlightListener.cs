using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightListener : MonoBehaviour, IPointerEnterHandler
{
    private AudioManager _audio;
    
    void Awake()
    {
        _audio = AudioManager.Instance;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _audio.PlayOneShot(AudioID.SFX.Interface.Inventory.hover);
    }
}
