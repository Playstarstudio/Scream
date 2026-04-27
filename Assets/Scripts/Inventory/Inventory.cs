using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{

    public class Inventory : MonoBehaviour, IInventory
    {
        HashSet<int> items = new HashSet<int>();

        public void AddToInventory(int itemID)
        {
            items.Add(itemID);
        }

        public bool HasItem(int itemId)
        {
            return items.Contains(itemId);
        }

        public void RemoveFromInventory(int itemID)
        {
            items.Remove(itemID);
        }

    }
}
