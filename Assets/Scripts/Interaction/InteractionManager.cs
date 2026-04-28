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
        if (characterMover == null) characterMover = FindFirstObjectByType<CharacterMovement>();

        if (characterMover != null)
            characterMover.InteractPressed += OnInteractPressed;
    }

    private void OnInteractPressed(object sender, EventArgs e)
    {
        IInteractable best = GetBestInteractable();
        best?.Interact();
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
            _nearbyInteractables.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
            _nearbyInteractables.Remove(interactable);
    }
}
