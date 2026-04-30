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
                    currentItems[i] = Instantiate(itemID.gameObject);
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
                if (currentItems[i] == null)
                {
                    // if there's an empty slot, we got room
                    return false;
                }
            }
            
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

        public void RemoveFromInventory(int itemId)
        {
            Debug.Log("Removed item with ID " + currentItems[itemId].name + " from inventory");
            draggableItems[itemId].RemoveItem();
            currentItems[itemId] = null;
        }
        public void OnDrop(int itemId)
        {
            KeyItem keyItem = currentItems[itemId].GetComponentInChildren<KeyItem>();
            Vector3 spawnPos = FindFirstObjectByType<CharacterMovement>().transform.position;
            GameObject worldItem = Instantiate(keyItem.gameObject, spawnPos, Quaternion.identity);
            worldItem.transform.parent = null;
            RemoveFromInventory(itemId);
        }

    }
}
