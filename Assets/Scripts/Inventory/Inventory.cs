using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Inventory
{

    public class Inventory : MonoBehaviour, IInventory
    {
        //public List<int> items = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 };
        public List<int> items = new List<int>() { -1 };

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
            _size = items.Count;
        }

        public bool AddToInventory(int itemID)
        {
            // no empty slots 
            if (!items.Contains(-1))
            {
                OnInventoryFull(null);
                return false;
            }

            // add to first empty slot
            items[items.IndexOf(-1)] = itemID;
            Debug.Log("Added item with ID " + itemID + " to inventory");
            return true;

        }

        public bool HasItem(int itemId)
        {
            return items.Contains(itemId);
        }

        public void RemoveFromInventory(int itemID)
        {
            items[items.IndexOf(itemID)] = -1;
            Debug.Log("Removed item with ID " + itemID + " from inventory");
        }

    }
}
