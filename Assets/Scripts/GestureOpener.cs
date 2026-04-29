using UnityEngine;
using UnityEngine.SceneManagement;

public class GestureOpener : MonoBehaviour, IInteractable
{
    public int InteractionPriority => 3;
    public bool CanInteract => true;

    [Tooltip("The name of the UI panel GameObject to toggle when the player interacts.")]
    public string uiPanelName;

    private GameObject _uiPanel;

    private void Start()
    {
        if (!string.IsNullOrEmpty(uiPanelName))
        {
            _uiPanel = FindInScene(uiPanelName);
            if (_uiPanel == null)
            {
                Debug.LogWarning($"[GestureOpener] Could not find GameObject named '{uiPanelName}'.");
            }
        }
    }

    public void Interact()
    {
        if (_uiPanel == null) return;

        bool isActive = _uiPanel.activeInHierarchy;
        _uiPanel.SetActive(!isActive);
    }
    
    private static GameObject FindInScene(string objectName)
    {
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name == objectName) return root;

            Transform found = FindChildRecursive(root.transform, objectName);
            if (found != null) return found.gameObject;
        }

        return null;
    }

    private static Transform FindChildRecursive(Transform parent, string objectName)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == objectName) return child;

            Transform found = FindChildRecursive(child, objectName);
            if (found != null) return found;
        }

        return null;
    }
}
