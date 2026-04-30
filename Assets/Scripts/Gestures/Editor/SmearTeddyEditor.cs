using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SmearTeddy))]
    public class SmearTeddyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SmearTeddy smearTeddy = (SmearTeddy)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Testing", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to use the testing buttons.", MessageType.Info);
                return;
            }

            // Status
            EditorGUILayout.LabelField("Teddy Placed", smearTeddy.IsTeddyPlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Teddy Smeared", smearTeddy.IsTeddySmeared ? "Yes" : "No");

            EditorGUILayout.Space(4);

            // Buttons
            EditorGUI.BeginDisabledGroup(smearTeddy.IsTeddyPlaced);
            if (GUILayout.Button("Place Teddy", GUILayout.Height(28)))
            {
                smearTeddy.PlaceTeddy();
            }
            EditorGUI.EndDisabledGroup();

            Repaint();
        }
    }
}

