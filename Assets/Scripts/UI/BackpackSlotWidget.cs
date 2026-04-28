using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackSlotWidget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;
        
        DraggableItemWidget draggableItem = dropped.GetComponent<DraggableItemWidget>();
        if (draggableItem == null) return;
        draggableItem.parentAfterDrag = transform;
    }

}
