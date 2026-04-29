using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // Script used for drag & dropping Inventory Items!

    public Image _image;
    public GameObject invItem;
    public int invItemIndex;
    public Transform parentAfterDrag;
    Transform rootTransform;
    public GameObject canvasParent;

    private void Awake()
    {
        canvasParent = GameObject.Find("OpenBackpackCanvas");
        rootTransform = canvasParent.GetComponent<Transform>();
        // _image = GetComponent<Image>();
        _image.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        parentAfterDrag = transform.parent;

        transform.SetParent(rootTransform); // set parent to canvas
        transform.SetAsLastSibling(); // move icon to top

        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        _image.raycastTarget = true;
    }

    public void AddItem(GameObject item, int index)
    {
        _image.enabled = true;
        invItem = item;
        _image.sprite = invItem.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = index;
    }

    public void RemoveItem(GameObject item)
    {
        _image.enabled = false;
        invItem = null;
        _image.sprite = null;
        invItemIndex = -1;
    }
}
