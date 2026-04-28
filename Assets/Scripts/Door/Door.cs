using UnityEngine;
using System;
using Services;


[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
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

    // The scene this door leads to. Used by SceneTransitionManager to find
    // the arrival door (the one whose target points back to where we came from)
    public string TargetSceneName => targetSceneName;
    
    public Vector3 SpawnPosition => transform.position + ScaleLocal(spawnOffset);
    
    public Vector2 FacingDirection => ((Vector2)ScaleLocal(facingDirection)).normalized;

    private Vector3 ScaleLocal(Vector2 local)
    {
        Vector3 s = transform.lossyScale;
        return new Vector3(local.x * s.x, local.y * s.y, 0f);
    }

    private bool _inTrigger;
    private Inventory.IInventory _inventory;

    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory.Inventory>();
        CharacterMovement chmov = FindFirstObjectByType<CharacterMovement>();
        if (chmov != null)
            chmov.InteractPressed += OnInteractPressed;
    }

    private void OnInteractPressed(object sender, EventArgs e)
    {
        if (!_inTrigger) return;

        // Optional key / item check
        if (requiredItemId >= 0)
        {
            if (_inventory == null || !_inventory.HasItem(requiredItemId))
            {
                Debug.Log($"[Door] Player does not have required item {requiredItemId}.");
                return;
            }
        }

        ServiceLocator.Instance.Get<SceneTransitionManager>().TransitionToScene(targetSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) _inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) _inTrigger = false;
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
