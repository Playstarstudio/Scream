using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItemWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // Script used for drag & dropping Inventory Items!

    public RectTransform cursorTransform;
    public Image cursorImage;
    public Canvas parentCanvas;

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
        var clickedSprite = _image.sprite;
        if (clickedSprite != null)
        {
            cursorImage.sprite = clickedSprite;
            cursorImage.gameObject.SetActive(true);
        }
        
        
        // parentAfterDrag = transform.parent;
        //
        // transform.SetParent(rootTransform); // set parent to canvas
        // transform.SetAsLastSibling(); // move icon to top
        //
        // _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        // Converts screen point (mouse) to local point relative to the Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, 
            Mouse.current.position.value,
            parentCanvas.worldCamera, 
            out position);
            
        cursorTransform.anchoredPosition = position;
        // transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cursorImage.gameObject.SetActive(false);
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
        if (hitObject != null)
        {
            var targetSlot = hitObject.GetComponent<DraggableItemWidget>()
                                       ?? hitObject.transform.parent.GetComponentInChildren<DraggableItemWidget>();

            if (targetSlot != null)
            {
                targetSlot.AddItem(invItem, invItemIndex);
                
                RemoveItem();
            }
        }
        
        // transform.SetParent(parentAfterDrag);
        // _image.raycastTarget = true;
        //
        // var itemRect = GetComponent<RectTransform>();
        // if (itemRect != null)
        // {
        //     itemRect.anchoredPosition = Vector2.zero;
        // }

    }

    public void AddItem(GameObject item, int index)
    {
        _image.enabled = true;
        invItem = item;
        _image.sprite = invItem.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = index;
    }

    public void RemoveItem()
    {
        _image.enabled = false;
        invItem = null;
        _image.sprite = null;
        invItemIndex = -1;
    }
}
