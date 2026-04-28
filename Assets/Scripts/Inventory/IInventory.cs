using System;

namespace Inventory
{
    public interface IInventory
    {
        public bool HasItem(int itemId);

        public bool AddToInventory(KeyItem itemID);

        public void RemoveFromInventory(int itemID);

        public event EventHandler<EventArgs> InventoryFull;

    }
}

