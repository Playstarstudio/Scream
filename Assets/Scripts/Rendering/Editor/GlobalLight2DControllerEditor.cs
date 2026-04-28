using UnityEditor;
using UnityEngine;

namespace Rendering.Editor
{
    [CustomEditor(typeof(GlobalLight2DController))]
    public class GlobalLight2DControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GlobalLight2DController controller = (GlobalLight2DController)target;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Lighting Mode", EditorStyles.boldLabel);

            bool isEditorMode = controller.UseEditorMode;
            string currentMode = Application.isPlaying
                ? "Game (Play Mode)"
                : isEditorMode ? "Editor (bright)" : "Game Preview (dark)";

            EditorGUILayout.HelpBox($"Current mode: {currentMode}", MessageType.Info);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            string buttonLabel = isEditorMode
                ? "Switch to Game Preview"
                : "Switch to Editor Mode";

            if (GUILayout.Button(buttonLabel, GUILayout.Height(28)))
            {
                Undo.RecordObject(controller, "Toggle Global Light Mode");
                controller.UseEditorMode = !isEditorMode;
                EditorUtility.SetDirty(controller);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            
            DrawDefaultInspector();
        }
    }
}

