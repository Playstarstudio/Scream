using UnityEngine;

public interface IInteractable
{
    int InteractionPriority { get; }
    
    bool CanInteract { get; }
    
    void Interact();
}

