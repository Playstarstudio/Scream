using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionManager : MonoBehaviour
{
    private readonly HashSet<IInteractable> _nearbyInteractables = new HashSet<IInteractable>();

    private void Start()
    {
        CharacterMovement characterMover = GetComponentInParent<CharacterMovement>();
        Debug.Log($"[InteractionManager] GetComponentInParent<CharacterMovement>: {(characterMover != null ? characterMover.name : "null")}");

        if (characterMover == null) characterMover = FindFirstObjectByType<CharacterMovement>();
        Debug.Log($"[InteractionManager] Final CharacterMovement ref: {(characterMover != null ? characterMover.name : "null")}");

        if (characterMover != null)
        {
            characterMover.InteractPressed += OnInteractPressed;
            Debug.Log("[InteractionManager] Subscribed to InteractPressed.");
        }
        else
        {
            Debug.LogError("[InteractionManager] Could not find CharacterMovement! Interactions will not work.");
        }
    }

    private void OnInteractPressed(object sender, EventArgs e)
    {
        Debug.Log("[InteractionManager] InteractPressed fired.");

        var hud = HUDSingleton.Instance;
        if (hud != null && hud.IsGestureScreenOpen)
        {
            Debug.Log("[InteractionManager] Gesture screen is open — closing it.");
            hud.CloseGestureScreen();
            return;
        }

        IInteractable best = GetBestInteractable();
        if (best != null)
        {
            Debug.Log($"[InteractionManager] Interacting with: {(best as MonoBehaviour)?.gameObject.name} (Priority: {best.InteractionPriority})");
            best.Interact();
        }
        else
        {
            Debug.Log($"[InteractionManager] No interactable found. Nearby count: {_nearbyInteractables.Count}");
        }
    }

    private IInteractable GetBestInteractable()
    {
        IInteractable best = null;
        int bestPriority = int.MinValue;

        foreach (var interactable in _nearbyInteractables)
        {
            if ((interactable as MonoBehaviour) == null) continue;
            if (!interactable.CanInteract) continue;

            if (interactable.InteractionPriority > bestPriority)
            {
                bestPriority = interactable.InteractionPriority;
                best = interactable;
            }
        }

        return best;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            _nearbyInteractables.Add(interactable);
            Debug.Log($"[InteractionManager] Entered range: {collision.gameObject.name} (Priority: {interactable.InteractionPriority}, CanInteract: {interactable.CanInteract})");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            _nearbyInteractables.Remove(interactable);
            Debug.Log($"[InteractionManager] Exited range: {collision.gameObject.name}");
        }
    }
}
