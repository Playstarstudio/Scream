using System.Collections.Generic;
using Inventory;

namespace RoomLayout.Editor
{
    /// <summary>
    /// A lightweight in-editor fake inventory backed by a plain list of item IDs.
    /// Used by <see cref="RoomLayoutSwitcherEditor"/> to preview key item filtering
    /// without needing a live PlayerInventory in the scene.
    /// </summary>
    internal class EditorFakeInventory : IInventory
    {
        private readonly List<int> _ownedItems;

        public EditorFakeInventory(List<int> ownedItems)
        {
            _ownedItems = ownedItems;
        }

        public bool HasItem(int itemId) => _ownedItems.Contains(itemId);
    }
}
