using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(OpenDoor))]
    public class OpenDoorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OpenDoor openDoor = (OpenDoor)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Testing", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to use the testing buttons.", MessageType.Info);
                return;
            }

            // Status
            EditorGUILayout.LabelField("Key Placed", openDoor.IsKeyPlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Door Opened", openDoor.IsDoorOpened ? "Yes" : "No");

            EditorGUILayout.Space(4);

            // Buttons
            EditorGUI.BeginDisabledGroup(openDoor.IsKeyPlaced);
            if (GUILayout.Button("Place Key", GUILayout.Height(28)))
            {
                openDoor.PlaceKey();
            }
            EditorGUI.EndDisabledGroup();

            Repaint();
        }
    }
}

