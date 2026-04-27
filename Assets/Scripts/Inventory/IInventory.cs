namespace Inventory
{
    public interface IInventory
    {
        public bool HasItem(int itemId);

        public void AddToInventory(int itemID);

        public void RemoveFromInventory(int itemID);

    }
}

