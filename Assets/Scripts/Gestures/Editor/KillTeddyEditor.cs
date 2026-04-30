using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(KillTeddy))]
    public class KillTeddyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            KillTeddy killTeddy = (KillTeddy)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Testing", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to use the testing buttons.", MessageType.Info);
                return;
            }

            // Status
            EditorGUILayout.LabelField("Teddy Placed", killTeddy.IsTeddyPlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Knife Placed", killTeddy.IsKnifePlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Teddy Killed", killTeddy.IsTeddyKilled ? "Yes" : "No");

            EditorGUILayout.Space(4);

            // Buttons
            EditorGUI.BeginDisabledGroup(killTeddy.IsTeddyPlaced);
            if (GUILayout.Button("Place Teddy", GUILayout.Height(28)))
            {
                killTeddy.PlaceTeddy();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(killTeddy.IsKnifePlaced);
            if (GUILayout.Button("Place Knife", GUILayout.Height(28)))
            {
                killTeddy.PlaceKnife();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(killTeddy.IsTeddyPlaced && killTeddy.IsKnifePlaced);
            if (GUILayout.Button("Place Both", GUILayout.Height(28)))
            {
                if (!killTeddy.IsTeddyPlaced) killTeddy.PlaceTeddy();
                if (!killTeddy.IsKnifePlaced) killTeddy.PlaceKnife();
            }
            EditorGUI.EndDisabledGroup();

            Repaint();
        }
    }
}

