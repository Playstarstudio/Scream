using Services;
using UnityEngine;

namespace UI
{
    public class RitualAmuletPlace : MonoBehaviour
    {
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
        private bool _ritualComplete;

        public bool IsRitualComplete => _ritualComplete;

        private void Awake()
        {
            _audio = AudioManager.Instance;
        }

        private void Start()
        {
            Debug.Log($"[RitualAmuletPlace] Start() — panel={panel?.name ?? "null"}, " +
                      $"amuletObject={amuletObject?.name ?? "null"}");

            if (amuletObject != null) amuletObject.SetActive(false);
        }

        public void PlaceAmulet()
        {
            Debug.Log($"[RitualAmuletPlace] PlaceAmulet() called. Already complete: {_ritualComplete}");
            if (_ritualComplete) return;

            _ritualComplete = true;

            if (amuletObject != null) amuletObject.SetActive(true);
            Debug.Log("[RitualAmuletPlace] Amulet placed. amuletObject active.");

            if (amuletConsumedStateKey != null)
            {
                Debug.Log($"[RitualAmuletPlace] Setting consumed state '{amuletConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(amuletConsumedStateKey, true);
            }

            // Placing the amulet completes the ritual
            Debug.Log("[RitualAmuletPlace] Ritual complete — closing gesture UI and setting state.");
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
