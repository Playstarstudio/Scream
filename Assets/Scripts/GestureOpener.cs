using System;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class GestureOpener : MonoBehaviour, IInteractable
{
    public int InteractionPriority => 3;
    public bool CanInteract => !string.IsNullOrEmpty(EvaluatePanels());

    [Serializable]
    public struct StateCondition
    {
        [Tooltip("The game state key to check.")]
        public GameStateKey key;
        [Tooltip("The required value for this key.")]
        public bool requiredValue;
    }

    [Serializable]
    public struct ConditionalGesturePanel
    {
        [Tooltip("The name of the UI panel to open (must match an entry in HUDSingleton's Panels list).")]
        public string panelName;
        [Tooltip("All conditions must be met for this panel to activate. If empty, it always matches.")]
        public List<StateCondition> conditions;
    }

    [Tooltip("Ordered list of gesture panels. The first one whose conditions are all met will activate.")]
    [SerializeField]
    private List<ConditionalGesturePanel> gesturePanels = new List<ConditionalGesturePanel>();

    public void Interact()
    {
        Debug.Log($"[GestureOpener] Interact() called on {gameObject.name}");

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
            return;
        }

        string panelToOpen = EvaluatePanels();

        if (string.IsNullOrEmpty(panelToOpen))
        {
            Debug.Log("[GestureOpener] No gesture panel conditions met — nothing to open.");
            return;
        }

        Debug.Log($"[GestureOpener] Opening gesture screen: '{panelToOpen}'");
        hud.OpenGestureScreen(panelToOpen);
    }

    private string EvaluatePanels()
    {
        var gsm = ServiceLocator.Instance?.Get<GameStateManager>();
        if (gsm == null)
        {
            Debug.LogWarning("[GestureOpener] GameStateManager not available.");
            return null;
        }

        for (int i = 0; i < gesturePanels.Count; i++)
        {
            var entry = gesturePanels[i];

            if (string.IsNullOrEmpty(entry.panelName))
            {
                Debug.LogWarning($"[GestureOpener] Entry {i} has no panel name — skipping.");
                continue;
            }

            bool allMet = true;

            if (entry.conditions != null)
            {
                foreach (var condition in entry.conditions)
                {
                    if (condition.key == null)
                    {
                        Debug.LogWarning($"[GestureOpener] Entry '{entry.panelName}' has a null condition key — skipping condition.");
                        continue;
                    }

                    bool currentValue = gsm.GetState(condition.key);
                    if (currentValue != condition.requiredValue)
                    {
                        Debug.Log($"[GestureOpener] Entry '{entry.panelName}' failed: " +
                                  $"'{condition.key.name}' is {currentValue}, required {condition.requiredValue}");
                        allMet = false;
                        break;
                    }
                }
            }

            if (allMet)
            {
                Debug.Log($"[GestureOpener] Entry '{entry.panelName}' — all conditions met.");
                return entry.panelName;
            }
        }

        return null;
    }
}
