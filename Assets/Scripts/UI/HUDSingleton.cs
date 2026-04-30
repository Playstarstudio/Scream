using System;
using System.Collections.Generic;
using UnityEngine;

public class HUDSingleton : MonoBehaviour
{
    public static HUDSingleton Instance { get; private set; }

    [Serializable]
    public struct NamedPanel
    {
        public string name;
        public GameObject gameObject;
    }

    [Header("Panels")]
    [Tooltip("Add any GameObjects you want to enable/disable by name.")]
    [SerializeField]
    private List<NamedPanel> panels = new List<NamedPanel>();

    [Header("Gesture Screen")]
    [SerializeField]
    private GameObject gestureCanvas;

    private Dictionary<string, GameObject> _panelLookup;

    public bool IsGestureScreenOpen { get; private set; }

    public event Action<bool> OnGestureScreenChanged;

    private string _activeGesturePanelName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildLookup();
    }

    private void BuildLookup()
    {
        _panelLookup = new Dictionary<string, GameObject>(panels.Count);
        Debug.Log($"[HUDSingleton] Building panel lookup from {panels.Count} entries...");
        foreach (var entry in panels)
        {
            if (string.IsNullOrEmpty(entry.name) || entry.gameObject == null)
            {
                Debug.LogWarning($"[HUDSingleton] Skipping invalid entry (name='{entry.name}', go={(entry.gameObject != null ? entry.gameObject.name : "null")})");
                continue;
            }

            if (!_panelLookup.ContainsKey(entry.name))
            {
                _panelLookup[entry.name] = entry.gameObject;
                Debug.Log($"[HUDSingleton] Registered panel: '{entry.name}' -> {entry.gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"[HUDSingleton] Duplicate panel name '{entry.name}' — skipping.");
            }
        }
        Debug.Log($"[HUDSingleton] Panel lookup complete. {_panelLookup.Count} panels registered.");
    }

    public void Show(string panelName)
    {
        if (TryGetPanel(panelName, out GameObject panel))
        {
            panel.SetActive(true);
        }
    }

    public void Hide(string panelName)
    {
        if (TryGetPanel(panelName, out GameObject panel))
        {
            panel.SetActive(false);
        }
    }

    public bool Toggle(string panelName)
    {
        if (TryGetPanel(panelName, out GameObject panel))
        {
            bool newState = !panel.activeSelf;
            panel.SetActive(newState);
            return newState;
        }

        return false;
    }

    public bool IsActive(string panelName)
    {
        if (TryGetPanel(panelName, out GameObject panel))
        {
            return panel.activeInHierarchy;
        }

        return false;
    }

    public bool TryGetPanel(string panelName, out GameObject panel)
    {
        if (_panelLookup != null && _panelLookup.TryGetValue(panelName, out panel))
        {
            return true;
        }

        Debug.LogWarning($"[HUDSingleton] Panel '{panelName}' not found. Registered panels: {(_panelLookup != null ? string.Join(", ", _panelLookup.Keys) : "lookup is null")}");
        panel = null;
        return false;
    }

    /// <summary>
    /// Opens a gesture panel and the gesture canvas together. Blocks movement.
    /// </summary>
    public void OpenGestureScreen(string panelName)
    {
        Debug.Log($"[HUDSingleton] OpenGestureScreen('{panelName}')");
        if (gestureCanvas != null) gestureCanvas.SetActive(true);
        Show(panelName);
        _activeGesturePanelName = panelName;
        SetGestureScreenOpen(true);
    }

    /// <summary>
    /// Closes the currently open gesture panel and the gesture canvas. Restores movement.
    /// </summary>
    public void CloseGestureScreen()
    {
        Debug.Log($"[HUDSingleton] CloseGestureScreen() — activePanel='{_activeGesturePanelName}'");
        if (!string.IsNullOrEmpty(_activeGesturePanelName))
        {
            Hide(_activeGesturePanelName);
            _activeGesturePanelName = null;
        }

        if (gestureCanvas != null) gestureCanvas.SetActive(false);
        SetGestureScreenOpen(false);
    }

    private void SetGestureScreenOpen(bool open)
    {
        if (IsGestureScreenOpen == open) return;
        Debug.Log($"[HUDSingleton] IsGestureScreenOpen changed: {IsGestureScreenOpen} -> {open}");
        IsGestureScreenOpen = open;
        OnGestureScreenChanged?.Invoke(open);
    }
}