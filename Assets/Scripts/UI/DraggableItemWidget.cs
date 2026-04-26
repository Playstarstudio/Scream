using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItemWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // Script used for drag & dropping Inventory Items!

    public Image image;
    public Transform parentAfterDrag;
    Transform rootTransform;

    private void Awake()
    {
        GameObject canvasParent = GameObject.Find("OpenBackpackCanvas");
        rootTransform = canvasParent.GetComponent<Transform>();
        image = transform.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        parentAfterDrag = transform.parent;

        transform.SetParent(rootTransform); // set parent to canvas
        transform.SetAsLastSibling(); // move icon to top

        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

    }
}
