using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(FirstAltarGestureScript))]
    public class FirstAltarGestureEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            FirstAltarGestureScript altar = (FirstAltarGestureScript)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug / Testing", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to use the testing buttons.", MessageType.Info);
                return;
            }

            // Status
            EditorGUILayout.LabelField("Matches Placed", altar.IsMatchesPlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Candle Placed", altar.IsCandlePlaced ? "Yes" : "No");
            EditorGUILayout.LabelField("Matches Lit", altar.IsMatchesLit ? "Yes" : "No");

            EditorGUILayout.Space(4);

            // Buttons
            EditorGUI.BeginDisabledGroup(altar.IsMatchesPlaced);
            if (GUILayout.Button("Place Matches", GUILayout.Height(28)))
            {
                altar.PlaceMatches();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(altar.IsCandlePlaced);
            if (GUILayout.Button("Place Candle", GUILayout.Height(28)))
            {
                altar.PlaceCandle();
            }
            EditorGUI.EndDisabledGroup();

            Repaint();
        }
    }
}

