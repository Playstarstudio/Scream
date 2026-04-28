using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
[ExecuteAlways]
public class GlobalLight2DController : MonoBehaviour
{
    [Header("Game Lighting (runtime)")]
    [Tooltip("Light intensity used during gameplay.")]
    [SerializeField, Range(0f, 2f)] private float gameIntensity = 0.05f;

    [Tooltip("Light color used during gameplay.")]
    [SerializeField] private Color gameColor = Color.white;

    [Header("Editor Lighting (level-editing)")]
    [Tooltip("Light intensity used while editing the level.")]
    [SerializeField, Range(0f, 2f)] private float editorIntensity = 1f;

    [Tooltip("Light color used while editing the level.")]
    [SerializeField] private Color editorColor = Color.white;

    [Header("Mode")]
    [Tooltip("When true (and outside Play Mode) the light uses the bright editor values.")]
    [SerializeField] private bool useEditorMode = true;

    private Light2D _light;

    public bool UseEditorMode
    {
        get => useEditorMode;
        set
        {
            useEditorMode = value;
            ApplyCurrentMode();
        }
    }


    public float GameIntensity => gameIntensity;
    public Color GameColor => gameColor;
    public float EditorIntensity => editorIntensity;
    public Color EditorColor => editorColor;

    private void OnEnable()
    {
        _light = GetComponent<Light2D>();
        _light.lightType = Light2D.LightType.Global;
        ApplyCurrentMode();
    }

    private void OnValidate()
    {
        if (_light == null) _light = GetComponent<Light2D>();
        ApplyCurrentMode();
    }
    
    public void ApplyCurrentMode()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && useEditorMode)
        {
            _light.intensity = editorIntensity;
            _light.color = editorColor;
            return;
        }
#endif
        _light.intensity = gameIntensity;
        _light.color = gameColor;
    }

    private void Reset()
    {
        _light = GetComponent<Light2D>();
        _light.lightType = Light2D.LightType.Global;
        ApplyCurrentMode();
    }
}

