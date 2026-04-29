using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class KeyItem : MonoBehaviour, IInteractable
{
    public int itemId;
    private SpriteRenderer sr;
    private CharacterMovement character;
    private Canvas charCanvas;
    [SerializeField] public Sprite sprite;
    private IInventory _inventory;
    public string textToRead;

    private void Awake()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        if (sr == null ) sr = this.gameObject.AddComponent<SpriteRenderer>();
        if ( sprite != null ) sr.sprite = sprite;
        _inventory = FindFirstObjectByType<Inventory.Inventory>();
    }

    private void Start()
    {
        character = FindFirstObjectByType<CharacterMovement>();
        charCanvas = character.GetComponentInChildren<Canvas>();
    }

    public int InteractionPriority => 10;

    public bool CanInteract => true;

    public void Interact()
    {
        if (_inventory != null && _inventory.AddToInventory(this))
        {
            TypewriterScript type = charCanvas.GetComponentInChildren<TypewriterScript>();
            type.SetText(textToRead);
            Destroy(gameObject);
        }
    }
}
