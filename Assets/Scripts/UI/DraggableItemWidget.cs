using System;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItemWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Script used for drag & dropping Inventory Items!

    public RectTransform cursorTransform;
    public Image cursorImage;
    public Canvas parentCanvas;

    public Image _image;
    public Sprite hiResSprite;
    public GameObject invItem;
    public int invItemIndex;
    public bool isZoomItem;
    public bool isHovered;
    public GameObject buttonPromptContainer;
    public Transform parentAfterDrag;
    Transform rootTransform;
    public GameObject canvasParent;
    
    private AudioManager _audio;

    private KeyItem invKeyItem;

    private void Awake()
    {
        canvasParent = GameObject.Find("OpenBackpackCanvas");
        rootTransform = canvasParent.GetComponent<Transform>();

        buttonPromptContainer = transform.parent.Find("ButtonPromptContainer").gameObject;
        isHovered = false;
        buttonPromptContainer.SetActive(false);

        // _image = GetComponent<Image>();
        _image.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isHovered)
        {
            NarrativeZoomScript zoomScript = FindFirstObjectByType<NarrativeZoomScript>();
            zoomScript.OpenZoomCanvas(hiResSprite);
        }
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

        if (_audio != null)
        {
            _audio.PlayOneShot(AudioID.SFX.Interface.Inventory.select);
        }
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
                return;
            }

            if (invKeyItem == null)
            {
                return;
            }

            // if (hitObject.CompareTag("Gesture"))
            if (true)
            {
                var killTeddy = hitObject.GetComponent<KillTeddy>();
                var altar = hitObject.GetComponent<FirstAltarGestureScript>();
                var amulet = hitObject.GetComponent<RitualAmuletPlace>();
                var smearTeddy = hitObject.GetComponent<SmearTeddy>();
                var door = hitObject.GetComponent<OpenDoor>();
                
                // MAGIC NUMBERS
                // 1 - matches
                // 2 - candle
                // 3 - skin
                // 4 - amulet
                // 5 - knife
                // 6 - locket
                // 7 - teddy
                
                if (killTeddy != null)
                {
                    if (invKeyItem.itemId == 7)
                    {
                        killTeddy.PlaceTeddy();
                        RemoveItem();
                    }
                    else if (invKeyItem.itemId == 5)
                    {
                        killTeddy.PlaceKnife();
                        RemoveItem();
                    }
                }
                else if (altar != null)
                {
                    if (invKeyItem.itemId == 2)
                    {
                        altar.PlaceCandle();
                        RemoveItem();
                    }
                    else if (invKeyItem.itemId == 1)
                    {
                        altar.PlaceMatches();
                        RemoveItem();
                    }
                }
                else if (amulet != null)
                {
                    if (invKeyItem.itemId == 4)
                    {
                        amulet.PlaceAmulet();
                        RemoveItem();
                    }
                }
                else if (smearTeddy != null)
                {
                    if (invKeyItem.itemId == 7)
                    {
                        smearTeddy.PlaceTeddy();
                        RemoveItem();
                    }
                }
                else if (door != null)
                {
                    if (invKeyItem.itemId == 9)
                    {
                        door.PlaceKey();
                        RemoveItem();
                    }
                }
            }
        }
        
        //_audio.PlayOneShot(AudioID.SFX.Interface.Inventory.unselect);
        
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
        invKeyItem = item.GetComponent<KeyItem>();
        _image.sprite = invItem.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = index;
        isZoomItem = item.GetComponent<KeyItem>().isZoomItem;
        hiResSprite = item.GetComponent<KeyItem>().hiResSprite;
    }

    public void RemoveItem()
    {
        _image.enabled = false;
        invItem = null;
        invKeyItem = null;
        _image.sprite = null;
        invItemIndex = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isZoomItem)
        {
            isHovered = true;
            buttonPromptContainer.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        buttonPromptContainer.SetActive(false);

    }
}
