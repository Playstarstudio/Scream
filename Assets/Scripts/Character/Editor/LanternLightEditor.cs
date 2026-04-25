using UnityEditor;
using UnityEngine;

namespace Character.Editor
{
    [CustomEditor(typeof(LanternLight))]
    public class LanternLightEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {

            LanternLight lantern = (LanternLight)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Lantern Setting Preview", EditorStyles.boldLabel);

            int count = lantern.LanternSettingsCount;
            int current = lantern.CurrentLanternSettingIndex;

            EditorGUILayout.HelpBox(
                $"Settings: {count}\nActive: [{current}]",
                count == 0 ? MessageType.Warning : MessageType.Info);

            EditorGUI.BeginDisabledGroup(count <= 1);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Prev Setting"))
            {
                int prev = (current - 1 + count) % count;
                SwitchToSetting(lantern, prev);
            }

            if (GUILayout.Button("Next Setting"))
            {
                int next = (current + 1) % count;
                SwitchToSetting(lantern, next);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();

            DrawDefaultInspector();
        }

        private void SwitchToSetting(LanternLight lantern, int index)
        {
            if (Application.isPlaying)
                lantern.TransitionToSetting(index);
            else
                lantern.SetLightingSettingsByIndex(index);

            EditorUtility.SetDirty(target);
        }
    }
}
