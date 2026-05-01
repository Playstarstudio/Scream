using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static AudioID.SFX.Interface;
using Object = UnityEngine.Object;

namespace Inventory
{
    
    public class Inventory : MonoBehaviour, IInventory
    {
        //public List<int> items = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 };
        public GameObject[] currentItems;
        public DraggableItemWidget[] draggableItems;

        public GameObject starterKey;

        public event EventHandler<EventArgs> InventoryFull;
        public void OnInventoryFull(EventArgs e)
        {
            if (InventoryFull != null)
            {
                InventoryFull(this, e);
            }

            Debug.Log("Inventory full! Did not add item");
        }

        private int _size;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _size = draggableItems.Length;
            currentItems = new GameObject[_size];
        }

        private void Start()
        {
            if (starterKey != null)
            {
                KeyItem starterKeyItem = starterKey.GetComponentInChildren<KeyItem>();
                if (starterKeyItem != null)
                {
                    AddToInventory(starterKeyItem);
                }
            }
        }

        private void AddKeyToInventory()
        {
            draggableItems[0].AddItem(currentItems[0], 0);
        }

        public bool AddToInventory(KeyItem itemID)
        {
            // no empty slots 
            if (IsInventoryFull())
            {
                OnInventoryFull(null);
                return false;
            }

            for (int i = 0; i < currentItems.Length; i++)
            {
                if (currentItems[i] == null)
                {
                    Debug.Log($"[Inventory] Adding '{itemID.gameObject.name}' (itemId={itemID.itemId}) to slot {i}");
                    currentItems[i] = Instantiate(itemID.gameObject);
                    currentItems[i].SetActive(false);
                    currentItems[i].transform.parent = gameObject.transform;
                    draggableItems[i].AddItem(currentItems[i], i);
                    break;
                }
            }
            return true;
        }
        public bool IsInventoryFull()
        {
            for (int i = 0; i < currentItems.Length; i++)
            {
                // Unity destroyed objects are == null but not 'is null'
                // Force-clean any destroyed references
                if (currentItems[i] != null && currentItems[i].Equals(null))
                {
                    Debug.Log($"[Inventory] Slot {i} had a destroyed reference — cleaning up.");
                    currentItems[i] = null;
                }
                
                if (currentItems[i] == null)
                {
                    return false;
                }
            }
            
            // Log what's occupying each slot
            for (int i = 0; i < currentItems.Length; i++)
            {
                Debug.Log($"[Inventory] Slot {i}: '{currentItems[i]?.name ?? "null"}' (active={currentItems[i]?.activeSelf})");
            }
            Debug.Log("[Inventory] Checked inventory space: no empty slots found.");
            return true;
        }

        public bool HasItem(int itemId)
        {
            bool foundItem = false;
            foreach (var item in currentItems.ToList())
            {
                if (item == null)
                    continue;
                else
                {
                    if (item.GetComponentInChildren<KeyItem>().itemId == itemId)
                        foundItem = true;
                }
            }
            return foundItem;
        }

        public void RemoveFromInventory(int slotIndex)
        {
            Debug.Log($"[Inventory] Removing slot {slotIndex}: '{currentItems[slotIndex]?.name ?? "null"}'");
            if (currentItems[slotIndex] != null)
            {
                Destroy(currentItems[slotIndex]);
            }
            currentItems[slotIndex] = null;
        }
        
        public void OnDrop(int slotIndex)
        {
            if (currentItems[slotIndex] == null) return;
            
            KeyItem keyItem = currentItems[slotIndex].GetComponentInChildren<KeyItem>();
            Vector3 spawnPos = FindFirstObjectByType<CharacterMovement>().transform.position;
            GameObject worldItem = Instantiate(keyItem.gameObject, spawnPos, Quaternion.identity);
            worldItem.transform.parent = null;
            
            // Clear inventory data
            RemoveFromInventory(slotIndex);
            
            // Clear widget visual
            if (draggableItems != null && slotIndex < draggableItems.Length)
            {
                draggableItems[slotIndex].ClearSlotVisual();
            }
        }
    }
}
