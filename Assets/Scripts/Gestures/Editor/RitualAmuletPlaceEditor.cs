using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(RitualAmuletPlace))]
    public class RitualAmuletPlaceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RitualAmuletPlace ritual = (RitualAmuletPlace)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Testing", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to use the testing buttons.", MessageType.Info);
                return;
            }

            // Status
            EditorGUILayout.LabelField("Amulet Placed", ritual.IsAmuletPlaced ? "✓ Yes" : "✗ No");
            EditorGUILayout.LabelField("Ritual Complete", ritual.IsRitualComplete ? "✓ Yes" : "✗ No");

            EditorGUILayout.Space(4);

            // Buttons
            EditorGUI.BeginDisabledGroup(ritual.IsAmuletPlaced);
            if (GUILayout.Button("Place Amulet", GUILayout.Height(28)))
            {
                ritual.PlaceAmulet();
            }
            EditorGUI.EndDisabledGroup();

            Repaint();
        }
    }
}

