using Services;
using UnityEngine;

namespace UI
{
    public class RitualAmuletPlace : MonoBehaviour
    {
        public DragGesture amuletPlaceGesture;
        public GameObject panel;

        [Header("World Objects")]
        [Tooltip("The amulet object in the world to show/hide based on placement state.")]
        public GameObject amuletObject;

        [Header("Game State")]
        public GameStateKey amuletPlacedStateKey;

        [Header("Consumed States")]
        [Tooltip("Set to true when the amulet is placed. Assign the same key to the amulet KeyItem's consumedStateKey.")]
        public GameStateKey amuletConsumedStateKey;

        private AudioManager _audio;
        private bool _amuletPlaced;
        private bool _ritualComplete;

        public bool IsAmuletPlaced => _amuletPlaced;
        public bool IsRitualComplete => _ritualComplete;

        private void Awake()
        {
            _audio = AudioManager.Instance;
        }

        private void Start()
        {
            Debug.Log($"[RitualAmuletPlace] Start() — panel={panel?.name ?? "null"}, " +
                      $"amuletObject={amuletObject?.name ?? "null"}, " +
                      $"gesture={amuletPlaceGesture?.name ?? "null"}");

            if (amuletObject != null) amuletObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            Debug.Log("[RitualAmuletPlace] OnEnable() — subscribing to gesture events.");
            if (amuletPlaceGesture == null) Debug.LogError("[RitualAmuletPlace] amuletPlaceGesture is null!");
            if (amuletPlaceGesture != null) amuletPlaceGesture.OnGestureEnd += OnAmuletPlaceGesture;
        }

        private void OnDisable()
        {
            Debug.Log("[RitualAmuletPlace] OnDisable() — unsubscribing from gesture events.");
            if (amuletPlaceGesture != null) amuletPlaceGesture.OnGestureEnd -= OnAmuletPlaceGesture;
        }

        public void PlaceAmulet()
        {
            Debug.Log($"[RitualAmuletPlace] PlaceAmulet() called. Already placed: {_amuletPlaced}");
            if (_amuletPlaced) return;

            _amuletPlaced = true;
            if (amuletObject != null) amuletObject.SetActive(true);
            Debug.Log("[RitualAmuletPlace] Amulet placed. amuletObject active.");

            if (amuletConsumedStateKey != null)
            {
                Debug.Log($"[RitualAmuletPlace] Setting consumed state '{amuletConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(amuletConsumedStateKey, true);
            }

            Debug.Log("[RitualAmuletPlace] Amulet placed — enabling gestures.");
            SetGesturesEnabled(true);
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            Debug.Log($"[RitualAmuletPlace] SetGesturesEnabled({isEnabled})");
            if (amuletPlaceGesture != null) amuletPlaceGesture.gameObject.SetActive(isEnabled);
        }

        private void OnAmuletPlaceGesture(DragDirection dragDirection)
        {
            Debug.Log($"[RitualAmuletPlace] OnAmuletPlaceGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Down)
                CompleteRitual();
        }

        private void CompleteRitual()
        {
            Debug.Log("[RitualAmuletPlace] CompleteRitual() — closing gesture UI and setting state.");
            _ritualComplete = true;
            GestureHelper.CloseGestureUI(panel);

            if (amuletPlacedStateKey != null)
            {
                Debug.Log($"[RitualAmuletPlace] Setting GameState '{amuletPlacedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(amuletPlacedStateKey, true);
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Amulet.place, GameObject.Find("Character"));
            }
            else
            {
                Debug.LogWarning("[RitualAmuletPlace] amuletPlacedStateKey is null — no game state set.");
            }
        }
    }
}
