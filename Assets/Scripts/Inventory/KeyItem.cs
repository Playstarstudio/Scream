using Inventory;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class KeyItem : MonoBehaviour, IInteractable
{
    public int itemId;
    private SpriteRenderer sr;
    [SerializeField] public Sprite sprite;
    private IInventory _inventory;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        _inventory = FindFirstObjectByType<Inventory.Inventory>();
    }

    public int InteractionPriority => 10;

    public bool CanInteract => true;

    public void Interact()
    {
        if (_inventory != null && _inventory.AddToInventory(this))
        {
            Destroy(gameObject);
        }
    }
}
