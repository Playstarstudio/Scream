using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackSlotWidget : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    private AudioManager _audio;
    
    void Awake()
    {
        _audio = AudioManager.Instance;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;
        
        DraggableItemWidget draggableItem = dropped.GetComponent<DraggableItemWidget>();
        if (draggableItem == null) return;
        draggableItem.parentAfterDrag = transform;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) _audio?.PlayOneShot(AudioID.SFX.Interface.Inventory.hover);
    }
}
