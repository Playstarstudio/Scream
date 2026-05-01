using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Inventory inventory = (Inventory)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Inventory State", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to see live inventory state.", MessageType.Info);
                return;
            }

            if (inventory.currentItems == null)
            {
                EditorGUILayout.HelpBox("currentItems array is null.", MessageType.Warning);
                Repaint();
                return;
            }

            EditorGUILayout.LabelField($"Slots: {inventory.currentItems.Length}");
            EditorGUILayout.Space(4);

            for (int i = 0; i < inventory.currentItems.Length; i++)
            {
                EditorGUILayout.BeginHorizontal("box");

                GameObject item = inventory.currentItems[i];
                bool isNull = item == null;
                bool isDestroyed = !isNull && item.Equals(null);

                string status;
                string itemName;
                string itemId;

                if (isNull)
                {
                    status = "EMPTY";
                    itemName = "—";
                    itemId = "—";
                }
                else if (isDestroyed)
                {
                    status = "DESTROYED (stale ref!)";
                    itemName = "(destroyed)";
                    itemId = "—";
                }
                else
                {
                    KeyItem keyItem = item.GetComponentInChildren<KeyItem>();
                    status = item.activeSelf ? "Active" : "Inactive";
                    itemName = item.name;
                    itemId = keyItem != null ? keyItem.itemId.ToString() : "no KeyItem";
                }

                // Slot number
                EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(30));

                // Status indicator
                Color prevColor = GUI.color;
                if (isNull)
                    GUI.color = Color.gray;
                else if (isDestroyed)
                    GUI.color = Color.red;
                else
                    GUI.color = Color.green;

                EditorGUILayout.LabelField("●", GUILayout.Width(15));
                GUI.color = prevColor;

                // Item info
                EditorGUILayout.LabelField($"{itemName}", GUILayout.MinWidth(120));
                EditorGUILayout.LabelField($"id={itemId}", GUILayout.Width(60));
                EditorGUILayout.LabelField(status, GUILayout.Width(160));

                // Widget info
                if (inventory.draggableItems != null && i < inventory.draggableItems.Length)
                {
                    var widget = inventory.draggableItems[i];
                    if (widget != null)
                    {
                        bool widgetHasItem = widget.invItem != null;
                        string widgetInfo = widgetHasItem
                            ? $"widget: '{widget.invItem.name}' idx={widget.invItemIndex}"
                            : "widget: empty";
                        EditorGUILayout.LabelField(widgetInfo, GUILayout.MinWidth(200));
                    }
                }

                // Action buttons
                if (!isNull && !isDestroyed)
                {
                    if (GUILayout.Button("Drop", GUILayout.Width(50)))
                    {
                        inventory.OnDrop(i);
                    }

                    if (GUILayout.Button("Clear", GUILayout.Width(50)))
                    {
                        inventory.RemoveFromInventory(i);
                        if (inventory.draggableItems != null && i < inventory.draggableItems.Length)
                        {
                            // Reset widget visuals without calling RemoveFromInventory again
                            var widget = inventory.draggableItems[i];
                            if (widget != null)
                            {
                                widget.invItem = null;
                                widget.invItemIndex = -1;
                            }
                        }
                    }
                }
                else if (isDestroyed)
                {
                    if (GUILayout.Button("Fix", GUILayout.Width(50)))
                    {
                        inventory.currentItems[i] = null;
                        Debug.Log($"[InventoryEditor] Cleared stale reference in slot {i}");
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            // Summary
            int occupied = 0;
            int empty = 0;
            int stale = 0;
            for (int i = 0; i < inventory.currentItems.Length; i++)
            {
                if (inventory.currentItems[i] == null)
                    empty++;
                else if (inventory.currentItems[i].Equals(null))
                    stale++;
                else
                    occupied++;
            }

            EditorGUILayout.LabelField($"Summary: {occupied} occupied, {empty} empty, {stale} stale", EditorStyles.miniLabel);

            if (stale > 0)
            {
                EditorGUILayout.HelpBox(
                    $"{stale} slot(s) have destroyed references that weren't cleaned up! Click 'Fix' to clear them.",
                    MessageType.Error);
            }

            // Force repaint every frame during play mode
            Repaint();
        }
    }
}

