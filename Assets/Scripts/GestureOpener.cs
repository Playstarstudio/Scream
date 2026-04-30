using UnityEngine;

public class GestureOpener : MonoBehaviour, IInteractable
{
    public int InteractionPriority => 3;
    public bool CanInteract => true;

    [Tooltip("The name of the UI panel to toggle when the player interacts.")]
    public string uiPanelName;

    public void Interact()
    {
        Debug.Log($"[GestureOpener] Interact() called on {gameObject.name}, uiPanelName='{uiPanelName}'");

        var hud = HUDSingleton.Instance;
        if (hud == null)
        {
            Debug.LogError("[GestureOpener] HUDSingleton.Instance is null!");
            return;
        }

        if (hud.IsGestureScreenOpen)
        {
            Debug.Log("[GestureOpener] Gesture screen is open — closing.");
            hud.CloseGestureScreen();
        }
        else
        {
            Debug.Log($"[GestureOpener] Opening gesture screen: '{uiPanelName}'");
            hud.OpenGestureScreen(uiPanelName);
        }
    }
}
