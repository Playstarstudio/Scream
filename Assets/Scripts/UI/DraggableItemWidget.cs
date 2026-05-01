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
    private Inventory.IInventory _inventory;
    private Inventory.Inventory _inventoryConcrete;
    
    public int slotIndex { get; private set; } = -1;

    private void Awake()
    {
        canvasParent = GameObject.Find("OpenBackpackCanvas");
        rootTransform = canvasParent.GetComponent<Transform>();

        buttonPromptContainer = transform.parent.Find("ButtonPromptContainer").gameObject;
        isHovered = false;
        buttonPromptContainer.SetActive(false);
        
        _audio = AudioManager.Instance;

        _image.enabled = false;
        
        FindInventoryAndSlot();
    }
    
    private void Start()
    {
        // Second chance in case Awake order didn't have Inventory ready
        if (slotIndex < 0)
        {
            FindInventoryAndSlot();
        }
        
        if (slotIndex < 0)
        {
            Debug.LogWarning($"[DraggableItemWidget] '{gameObject.name}' could not determine its slotIndex!");
        }
    }
    
    private void FindInventoryAndSlot()
    {
        if (_inventoryConcrete == null)
        {
            _inventoryConcrete = GetComponentInParent<Inventory.Inventory>();
            if (_inventoryConcrete == null)
            {
                _inventoryConcrete = FindFirstObjectByType<Inventory.Inventory>();
            }
            _inventory = _inventoryConcrete;
        }
        
        if (_inventoryConcrete != null && _inventoryConcrete.draggableItems != null && slotIndex < 0)
        {
            for (int i = 0; i < _inventoryConcrete.draggableItems.Length; i++)
            {
                if (_inventoryConcrete.draggableItems[i] == this)
                {
                    slotIndex = i;
                    Debug.Log($"[DraggableItemWidget] '{gameObject.name}' assigned slotIndex={i}");
                    break;
                }
            }
        }
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
        
        _audio?.PlayOneShot(AudioID.SFX.Interface.Inventory.select);
        
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
            _audio?.PlayOneShot(AudioID.SFX.Interface.Inventory.unselect);
            
            var targetSlot = hitObject.GetComponent<DraggableItemWidget>()
                                       ?? hitObject.transform.parent.GetComponentInChildren<DraggableItemWidget>();
            
            if (targetSlot != null && targetSlot != this)
            {
                // Move item in the inventory array: source slot -> target slot
                int srcSlot = slotIndex;
                int dstSlot = targetSlot.slotIndex;
                
                if (_inventoryConcrete != null && srcSlot >= 0 && dstSlot >= 0)
                {
                    // Move the GameObject reference in the inventory array
                    _inventoryConcrete.currentItems[dstSlot] = _inventoryConcrete.currentItems[srcSlot];
                    _inventoryConcrete.currentItems[srcSlot] = null;
                    
                    Debug.Log($"[DraggableItemWidget] Moved item from slot {srcSlot} to slot {dstSlot}");
                }
                
                // Update target widget with the item, using the TARGET's slot index
                targetSlot.SetItem(invItem, invKeyItem);
                
                ClearSlotVisual();
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
                    if (invKeyItem.itemId == 8)
                    {
                        smearTeddy.PlaceTeddy();
                        RemoveItem();
                    }
                }
                else if (door != null)
                {
                    if (invKeyItem.itemId == 9)
                    {
                        Debug.Log($"[DraggableItemWidget] Placing key on door from widget slotIndex={slotIndex}, invItemIndex={invItemIndex}, invItem={invItem?.name ?? "null"}");
                        door.PlaceKey();
                        RemoveItem();
                    }
                }
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
        invKeyItem = item.GetComponent<KeyItem>();
        _image.sprite = invItem.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = index;
        isZoomItem = item.GetComponent<KeyItem>().isZoomItem;
        hiResSprite = item.GetComponent<KeyItem>().hiResSprite;
    }
    
    /// <summary>
    /// Sets item on this widget using its own fixed slotIndex.
    /// Used for slot-to-slot moves where the inventory array is updated separately.
    /// </summary>
    public void SetItem(GameObject item, KeyItem keyItem)
    {
        _image.enabled = true;
        invItem = item;
        invKeyItem = keyItem;
        _image.sprite = item.GetComponent<SpriteRenderer>().sprite;
        invItemIndex = slotIndex; // Always use this widget's own slot
        isZoomItem = keyItem != null && keyItem.isZoomItem;
        hiResSprite = keyItem != null ? keyItem.hiResSprite : null;
    }

    /// <summary>
    /// Clears the widget visual only — used when moving items between slots.
    /// Does NOT destroy the inventory clone.
    /// </summary>
    public void ClearSlotVisual()
    {
        _image.enabled = false;
        invItem = null;
        invKeyItem = null;
        _image.sprite = null;
        invItemIndex = -1;
    }

    /// <summary>
    /// Removes the item and destroys it from inventory — used when consuming items (gesture placement, dropping).
    /// </summary>
    public void RemoveItem()
    {
        // Lazy-find inventory if not set
        if (_inventory == null)
        {
            _inventoryConcrete = GetComponentInParent<Inventory.Inventory>();
            _inventory = _inventoryConcrete;
            if (_inventory == null)
            {
                _inventoryConcrete = FindFirstObjectByType<Inventory.Inventory>();
                _inventory = _inventoryConcrete;
            }
        }

        // Use slotIndex (fixed) for inventory removal
        int removeSlot = slotIndex >= 0 ? slotIndex : invItemIndex;

        if (_inventory != null && removeSlot >= 0)
        {
            Debug.Log($"[DraggableItemWidget] RemoveItem() — calling RemoveFromInventory(slot {removeSlot})");
            _inventory.RemoveFromInventory(removeSlot);
        }
        else
        {
            Debug.LogWarning($"[DraggableItemWidget] RemoveItem() — inventory={(_inventory != null ? "found" : "NULL")}, slotIndex={slotIndex}, invItemIndex={invItemIndex}. Slot NOT freed!");
        }
        
        ClearSlotVisual();
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
