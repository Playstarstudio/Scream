using System;
using System.Collections.Generic;
using UnityEngine;
using Services;


[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour, IInteractable
{
    [Header("Connection")]
    [Tooltip("The name of the scene this door leads to.")]
    [SerializeField] private string targetSceneName;

    [Tooltip("Offset from the door where the player will spawn")]
    [SerializeField] private Vector2 spawnOffset = new Vector2(0f, -1f);

    [Tooltip("Direction the player should face when arriving")]
    [SerializeField] private Vector2 facingDirection = new Vector2(0f, -1f);

    [Header("Requirements (optional)")]
    [Tooltip("Item ID the player must have to use this door. Set to -1 for no requirement.")]
    [SerializeField] private int requiredItemId = -1;

    [Tooltip("All state conditions must be met to use this door. If empty, no state requirements.")]
    [SerializeField] private List<StateCondition> stateRequirements = new List<StateCondition>();

    [Serializable]
    public struct StateCondition
    {
        [Tooltip("The game state key to check.")]
        public GameStateKey key;
        [Tooltip("The required value for this key.")]
        public bool requiredValue;
    }

    public string TargetSceneName => targetSceneName;
    
    public Vector3 SpawnPosition => transform.position + ScaleLocal(spawnOffset);
    
    public Vector2 FacingDirection => ((Vector2)ScaleLocal(facingDirection)).normalized;

    private Vector3 ScaleLocal(Vector2 local)
    {
        Vector3 s = transform.lossyScale;
        return new Vector3(local.x * s.x, local.y * s.y, 0f);
    }
    
    public int InteractionPriority => 0; // Low priority — items should be picked up before doors are used

    public bool CanInteract
    {
        get
        {
            if (string.IsNullOrEmpty(targetSceneName)) return false;
            if (!AreStateRequirementsMet()) return false;
            if (!HasRequiredItem()) return false;
            return true;
        }
    }

    public void Interact()
    {
        ServiceLocator.Instance.Get<SceneTransitionManager>().TransitionToScene(targetSceneName);
    }

    private bool HasRequiredItem()
    {
        if (requiredItemId < 0) return true;
        var inventory = FindFirstObjectByType<Inventory.Inventory>() as Inventory.IInventory;
        return inventory != null && inventory.HasItem(requiredItemId);
    }

    private bool AreStateRequirementsMet()
    {
        if (stateRequirements == null || stateRequirements.Count == 0)
            return true;

        var gsm = ServiceLocator.Instance?.Get<GameStateManager>();
        if (gsm == null)
        {
            Debug.LogWarning("[Door] GameStateManager not available — skipping state check.");
            return true;
        }

        foreach (var condition in stateRequirements)
        {
            if (condition.key == null) continue;

            bool currentValue = gsm.GetState(condition.key);
            if (currentValue != condition.requiredValue)
            {
                Debug.Log($"[Door] Requirement failed: '{condition.key.name}' is {currentValue}, required {condition.requiredValue}");
                return false;
            }
        }

        return true;
    }

#if UNITY_EDITOR
    [Header("Editor Gizmo")]
    [SerializeField] private Color gizmoColor = Color.cyan;
    [SerializeField] private float gizmoRadius = 0.3f;

    private void OnDrawGizmos()
    {
        // Door
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
        string label = string.IsNullOrEmpty(targetSceneName) ? "(no target)" : targetSceneName;
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.4f, $"Door to {label}");

        // Spawn point
        Vector3 spawn = SpawnPosition;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawn, gizmoRadius * 0.6f);
        Gizmos.DrawLine(transform.position, spawn);
        UnityEditor.Handles.Label(spawn + Vector3.up * 0.3f, "Spawn");

        // Facing direction arrow from spawn point
        Vector2 worldDir = FacingDirection;
        if (worldDir.sqrMagnitude > 0.01f)
        {
            Gizmos.color = Color.yellow;
            Vector3 dir = (Vector3)worldDir * 0.5f;
            Vector3 tip = spawn + dir;
            Gizmos.DrawLine(spawn, tip);

            // Arrowhead
            Vector3 right = Quaternion.Euler(0, 0, 30) * -dir * 0.3f;
            Vector3 left  = Quaternion.Euler(0, 0, -30) * -dir * 0.3f;
            Gizmos.DrawLine(tip, tip + right);
            Gizmos.DrawLine(tip, tip + left);
        }
    }
#endif
}
