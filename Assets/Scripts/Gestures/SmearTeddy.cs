using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SmearTeddy : MonoBehaviour
    {
        public DragGesture teddyDragGesture;
        public GameObject panel;

        [Header("World Objects")]
        [Tooltip("The teddy object in the world to show/hide based on placement state.")]
        public GameObject teddyObject;

        [Header("Game State")]
        public GameStateKey teddyDraggedStateKey;

        [Header("Consumed States")]
        [Tooltip("Set to true when the teddy is placed. Assign the same key to the teddy KeyItem's consumedStateKey.")]
        public GameStateKey teddyConsumedStateKey;

        private AudioManager _audio;
        private bool _teddyPlaced;
        private bool _teddySmeared;

        public bool IsTeddyPlaced => _teddyPlaced;
        public bool IsTeddySmeared => _teddySmeared;

        private void Awake()
        {
            _audio = AudioManager.Instance;
        }

        private void Start()
        {
            Debug.Log($"[SmearTeddy] Start() — panel={panel?.name ?? "null"}, " +
                      $"teddyObject={teddyObject?.name ?? "null"}, " +
                      $"gesture={teddyDragGesture?.name ?? "null"}");

            if (teddyObject != null) teddyObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            Debug.Log("[SmearTeddy] OnEnable() — subscribing to gesture events.");
            if (teddyDragGesture == null) Debug.LogError("[SmearTeddy] teddyDragGesture is null!");
            if (teddyDragGesture != null) teddyDragGesture.OnGestureEnd += OnTeddyDragGesture;
        }

        private void OnDisable()
        {
            Debug.Log("[SmearTeddy] OnDisable() — unsubscribing from gesture events.");
            if (teddyDragGesture != null) teddyDragGesture.OnGestureEnd -= OnTeddyDragGesture;
        }

        public void PlaceTeddy()
        {
            Debug.Log($"[SmearTeddy] PlaceTeddy() called. Already placed: {_teddyPlaced}");
            if (_teddyPlaced) return;

            _teddyPlaced = true;
            if (teddyObject != null) teddyObject.SetActive(true);
            Debug.Log("[SmearTeddy] Teddy placed. teddyObject active.");

            if (teddyConsumedStateKey != null)
            {
                Debug.Log($"[SmearTeddy] Setting consumed state '{teddyConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyConsumedStateKey, true);
            }

            Debug.Log("[SmearTeddy] Teddy placed — enabling gestures.");
            _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Teddy_Bear.place_bloody, GameObject.Find("Character"));
            SetGesturesEnabled(true);
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            Debug.Log($"[SmearTeddy] SetGesturesEnabled({isEnabled})");
            if (teddyDragGesture != null) teddyDragGesture.gameObject.SetActive(isEnabled);
        }

        private void OnTeddyDragGesture(DragDirection dragDirection)
        {
            Debug.Log($"[SmearTeddy] OnTeddyDragGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Right)
                SmearTheTeddy();
                // TODO: Why doesn't this play
                // _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Teddy_Bear.smear, GameObject.Find("Character"));
        }

        private void SmearTheTeddy()
        {
            Debug.Log("[SmearTeddy] SmearTheTeddy() — closing gesture UI and setting state.");
            _teddySmeared = true;
            GestureHelper.CloseGestureUI(panel);

            if (teddyDraggedStateKey != null)
            {
                Debug.Log($"[SmearTeddy] Setting GameState '{teddyDraggedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyDraggedStateKey, true);
                SceneManager.LoadScene("CreditsScene");
            }
            else
            {
                Debug.LogWarning("[SmearTeddy] teddyDraggedStateKey is null — no game state set.");
            }
        }
    }
}
