using Inventory;
using Services;
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
    [SerializeField] public Sprite hiResSprite;
    private IInventory _inventory;
    public string textToRead;
    public bool isZoomItem;

    private NarrativeZoomScript zoomScript;

    [Header("Game State")]
    [Tooltip("Optional: When this state is true, the item is considered consumed and should not spawn.")]
    public GameStateKey consumedStateKey;

    /// Returns true if this item has been consumed (used in a gesture screen).
    public bool IsConsumed
    {
        get
        {
            if (consumedStateKey == null) return false;
            var gsm = ServiceLocator.Instance?.Get<GameStateManager>();
            return gsm != null && gsm.GetState(consumedStateKey);
        }
    }

    private void Awake()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        if (sr == null) sr = this.gameObject.AddComponent<SpriteRenderer>();
        if (sprite != null) sr.sprite = sprite;
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
            TypewriterScript[] typewriterArray = charCanvas.GetComponentsInChildren<TypewriterScript>();

            foreach (TypewriterScript typewriterScript in typewriterArray)
            {
                typewriterScript.SetText(textToRead);
            }

            if (isZoomItem)
            {
                zoomScript = FindFirstObjectByType<NarrativeZoomScript>();
                zoomScript.OpenZoomCanvas(hiResSprite);
            }

            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
