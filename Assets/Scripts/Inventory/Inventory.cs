using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Inventory
{

    public class Inventory : MonoBehaviour, IInventory
    {
        //public List<int> items = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 };
        public List<GameObject> invItems = new List<GameObject>();
        public List<DraggableItemWidget> draggableItems = new List<DraggableItemWidget>();

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
        private void Start()
        {
            _size = invItems.Count;
        }

        public bool AddToInventory(KeyItem itemID)
        {
            // no empty slots 
            if (IsInventoryFull())
            {
                OnInventoryFull(null);
                return false;
            }
            foreach (var item in invItems.ToList())
            {
                if (item != null)
                {
                    continue;
                }
                // add to first empty slot
                else
                {
                    int index = invItems.IndexOf(item);
                    invItems[index] = Instantiate(itemID.gameObject);
                    draggableItems[index].AddItem(invItems[index], index);
                    break;
                }
            }
            return true;
        }
        public bool IsInventoryFull()
        {
            int itemCount = 0;
            bool isFull = false;
            foreach (var item in invItems.ToList())
            {
                if (item != null)
                {
                    itemCount++;
                }
            }

            if (itemCount >= _size)
            {
                isFull = true;
            }
            else if (itemCount < _size)
            {
                isFull = false;
            }
            return isFull;
        }

        public bool HasItem(int itemId)
        {
            bool foundItem = false;
            foreach (var item in invItems.ToList())
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
            Debug.Log("Removed item with ID " + invItems[itemId].name + " from inventory");
            draggableItems[itemId].RemoveItem(invItems[itemId].gameObject);
            invItems[itemId] = null;
        }
    }
}
