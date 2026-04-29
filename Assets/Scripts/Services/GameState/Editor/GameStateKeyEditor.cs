using Services;
using UnityEditor;
using UnityEngine;

namespace Services.Editor
{
    [CustomEditor(typeof(GameStateKey))]
    public class GameStateKeyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Runtime State", EditorStyles.boldLabel);

            GameStateKey key = (GameStateKey)target;

            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle("Current Value", key.currentValue);
                EditorGUI.EndDisabledGroup();

                Repaint();
            }
            else
            {
                EditorGUILayout.HelpBox("Enter Play Mode to see the live state value.", MessageType.Info);
            }
        }
    }
}

