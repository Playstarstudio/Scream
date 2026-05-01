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
        // Lazy-find inventory every time — handles DontDestroyOnLoad and runtime-spawned items
        if (_inventory == null)
        {
            _inventory = FindFirstObjectByType<Inventory.Inventory>();
            if (_inventory == null)
            {
                Debug.LogWarning("[KeyItem] Could not find Inventory — item not picked up.");
                return;
            }
        }

        Debug.Log($"[KeyItem] Interact() on '{gameObject.name}' (itemId={itemId}). Inventory found: {_inventory != null}");

        if (_inventory.AddToInventory(this))
        {
            Debug.Log($"[KeyItem] Added to inventory successfully.");

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

            // Destroy the root object — if we have a parent, destroy the parent; otherwise destroy ourselves
            GameObject toDestroy = transform.parent != null ? transform.parent.gameObject : gameObject;
            Debug.Log($"[KeyItem] Destroying '{toDestroy.name}'");
            Destroy(toDestroy);
        }
        else
        {
            Debug.Log($"[KeyItem] AddToInventory returned false — inventory may be full.");
        }
    }
}
