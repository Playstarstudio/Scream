using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // Script used for drag & dropping Inventory Items!

    public Image image;
    public GameObject invItem;
    public int invItemIndex;
    public Transform parentAfterDrag;
    Transform rootTransform;
    public GameObject canvasParent;

    private void Awake()
    {
        canvasParent = GameObject.Find("OpenBackpackCanvas");
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

    public void AddItem(GameObject item, int index)
    {
        invItem = item;
        image.sprite = invItem.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = index;
    }

    public void RemoveItem(GameObject item)
    {
        invItem = null;
        image.sprite = null;
        invItemIndex = -1;
    }
}
