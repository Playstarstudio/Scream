using UnityEngine;
using Inventory;

[RequireComponent(typeof(Collider2D))]
public class KeyItem : MonoBehaviour, IInteractable
{
    public int itemId;

    private IInventory _inventory;

    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory.Inventory>();
    }

    public int InteractionPriority => 10;

    public bool CanInteract => true;

    public void Interact()
    {
        if (_inventory != null && _inventory.AddToInventory(itemId))
        {
            Destroy(gameObject);
        }
    }
}
