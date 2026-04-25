using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RoomLayout.Editor
{
    [CustomEditor(typeof(RoomLayoutSwitcher))]
    public class RoomLayoutSwitcherEditor : UnityEditor.Editor
    {
        private bool _fakeInventoryFoldout = true;
        private readonly List<int> _fakeOwnedItems = new List<int>();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoomLayoutSwitcher switcher = (RoomLayoutSwitcher)target;

            DrawLayoutSwitcher(switcher);

            DrawKnownKeyItems(switcher);
            
            DrawTestingInventory(switcher);
        }

        private void DrawTestingInventory(RoomLayoutSwitcher switcher)
        {
            // Fake Inventory
            EditorGUILayout.Space();
            _fakeInventoryFoldout = EditorGUILayout.Foldout(_fakeInventoryFoldout, "Testing Inventory", true, EditorStyles.foldoutHeader);

            if (_fakeInventoryFoldout)
            {
                EditorGUI.indentLevel++;
                
                EditorGUILayout.LabelField("Owned Item IDs", EditorStyles.miniLabel);

                int toRemove = -1;
                for (int i = 0; i < _fakeOwnedItems.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _fakeOwnedItems[i] = EditorGUILayout.IntField(_fakeOwnedItems[i]);
                    if (GUILayout.Button("-", GUILayout.Width(24)))
                        toRemove = i;
                    EditorGUILayout.EndHorizontal();
                }

                if (toRemove >= 0)
                {
                    _fakeOwnedItems.RemoveAt(toRemove);
                    switcher.ActivateLayout(switcher.CurrentLayoutIndex, BuildFakeInventory());
                }

                if (GUILayout.Button("+ Add Item ID"))
                {
                    _fakeOwnedItems.Add(0);
                    switcher.ActivateLayout(switcher.CurrentLayoutIndex, BuildFakeInventory());
                }

                if (GUILayout.Button("Clear All"))
                {
                    _fakeOwnedItems.Clear();
                    switcher.ActivateLayout(switcher.CurrentLayoutIndex, BuildFakeInventory());
                }

                EditorGUI.indentLevel--;
            }
        }

        private void DrawLayoutSwitcher(RoomLayoutSwitcher switcher)
        {
            // Layout Switcher
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Layout Switcher", EditorStyles.boldLabel);

            int count = switcher.LayoutCount;
            string currentName = (count > 0 && switcher.CurrentLayoutIndex >= 0)
                ? switcher.transform.GetChild(switcher.CurrentLayoutIndex).name
                : "—";

            EditorGUILayout.HelpBox(
                $"Layouts found: {count}\nActive: [{switcher.CurrentLayoutIndex}] {currentName}",
                count == 0 ? MessageType.Warning : MessageType.Info);

            EditorGUI.BeginDisabledGroup(count == 0);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Prev Layout"))
            {
                int prev = (switcher.CurrentLayoutIndex - 1 + count) % count;
                switcher.ActivateLayout(prev, BuildFakeInventory());
            }

            if (GUILayout.Button("Next Layout"))
            {
                switcher.SelectNextLayout(BuildFakeInventory());
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Random Layout"))
            {
                switcher.SelectRandomLayout(BuildFakeInventory());
            }

            EditorGUI.EndDisabledGroup();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void DrawKnownKeyItems(RoomLayoutSwitcher switcher)
        {
            if(switcher.CurrentLayoutIndex >= 0)
            {
                EditorGUILayout.LabelField("KeyItems in this layout:", EditorStyles.boldLabel);
                
                Transform layout = switcher.transform.GetChild(switcher.CurrentLayoutIndex);
                KeyItem[] keyItems = layout.GetComponentsInChildren<KeyItem>(includeInactive: true);
                if (keyItems.Length == 0)
                {
                    EditorGUILayout.HelpBox("No KeyItem components found in this child layout.", MessageType.None);
                    return;
                }


                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(16);
                EditorGUILayout.LabelField(
                    $"[{switcher.CurrentLayoutIndex}] {layout.name}: " + string.Join(", ", keyItems.Select(k => $"ID {k.itemId} ({k.gameObject.name})")),
                    EditorStyles.miniLabel);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(2);
        }

        private EditorFakeInventory BuildFakeInventory()
        {
            return new EditorFakeInventory(_fakeOwnedItems);
        }
    }
}
