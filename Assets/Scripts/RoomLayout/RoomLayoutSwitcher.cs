using Inventory;
using UnityEngine;

namespace RoomLayout
{
    public class RoomLayoutSwitcher : MonoBehaviour
    {
        [SerializeField] private int currentLayoutIndex = -1;

        // Total number of layout children
        public int LayoutCount => transform.childCount;

        // Index of the currently active layout
        public int CurrentLayoutIndex => currentLayoutIndex;

        private void Start()
        {
            SelectRandomLayout();
        }

        // Activates a random layout, deactivating all others.
        public void SelectRandomLayout(IInventory inventoryOverride = null)
        {
            if (LayoutCount == 0)
            {
                DebugEditor.LogWarning($"[RoomLayoutSwitcher] No child layouts found on '{gameObject.name}'.", this);
                return;
            }

            int index = Random.Range(0, LayoutCount);
            ActivateLayout(index, inventoryOverride);
        }

        // Cycles to the next layout
        public void SelectNextLayout(IInventory inventoryOverride = null)
        {
            if (LayoutCount == 0) return;

            int next = (currentLayoutIndex + 1) % LayoutCount;
            ActivateLayout(next, inventoryOverride);
        }

        // Activates the layout and disables all others.
        // Pass an inventory to prevent any key items from spawning
        public void ActivateLayout(int index, IInventory inventory = null)
        {
            if (index < 0 || index >= LayoutCount)
            {
                return;
            }

            for (int i = 0; i < LayoutCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == index);
            }

            currentLayoutIndex = index;

            FilterKeyItemsForInventory(transform.GetChild(index), inventory);
        }

        // Finds every KeyItem in the activated layout and disables any that the
        // player already has in their inventory.
        // ReSharper disable Unity.PerformanceAnalysis
        private void FilterKeyItemsForInventory(Transform layout, IInventory inventory = null)
        {
            if (inventory == null)
            {
                // TODO: pmoffett Update this once the inventory system is hooked up to the scene.
                DebugEditor.LogError($"[RoomLayoutSwitcher] No inventory provided to filter key items in layout '{layout.name}'. All key items will be active.", this);
                return;
            }

            KeyItem[] keyItems = layout.GetComponentsInChildren<KeyItem>(includeInactive: true);

            foreach (KeyItem item in keyItems)
            {
                if (inventory.HasItem(item.itemId))
                {
                    item.gameObject.SetActive(false);
                    DebugEditor.Log($"[RoomLayoutSwitcher] Disabled already-owned key item '{item.itemId}' in layout '{layout.name}'.", this);
                }
                else
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
    }
}
