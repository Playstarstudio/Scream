using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackSlotWidget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItemWidget draggableItem = dropped.GetComponent<DraggableItemWidget>();
        draggableItem.parentAfterDrag = transform;
    }

}
