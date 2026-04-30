using UnityEngine;
using Services;

namespace UI
{
    public class FirstAltarGestureScript : MonoBehaviour
    {
        public DragGesture openMatchesDragGesture;
        public DragGesture closeMatchesDragGesture;
        public DragGesture lightMatchesDragGesture;
        public GameObject panel;
        public GameObject closedMatches;
        public GameObject openMatches;

        [Header("World Objects")]
        [Tooltip("The match object in the world to show/hide based on match state.")]
        public GameObject matchObject;
        [Tooltip("The candle object in the world to show/hide when matches are lit.")]
        public GameObject candleObject;

        [Header("Game State")]
        public GameStateKey matchesLitStateKey;

        [Header("Consumed States")]
        [Tooltip("Set to true when matches are placed on the altar. Assign the same key to the matches KeyItem's consumedStateKey.")]
        public GameStateKey matchesConsumedStateKey;
        [Tooltip("Set to true when the candle is placed on the altar. Assign the same key to the candle KeyItem's consumedStateKey.")]
        public GameStateKey candleConsumedStateKey;

        private bool _matchesPlaced;
        private bool _candlePlaced;
        private bool _matchesOpen;
        private bool _matchesLit;
        private AudioManager _audio;

        private void Start()
        {
            Debug.Log($"[AltarGesture] Start() — panel={panel?.name ?? "null"}, " +
                      $"matchObject={matchObject?.name ?? "null"}, candleObject={candleObject?.name ?? "null"}, " +
                      $"openGesture={openMatchesDragGesture?.name ?? "null"}, closeGesture={closeMatchesDragGesture?.name ?? "null"}, lightGesture={lightMatchesDragGesture?.name ?? "null"}");

            // Both items start disabled until placed from inventory
            if (matchObject != null) matchObject.SetActive(false);
            if (candleObject != null) candleObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            Debug.Log("[AltarGesture] OnEnable() — subscribing to gesture events.");
            if (openMatchesDragGesture == null) Debug.LogError("[AltarGesture] openMatchesDragGesture is null!");
            if (closeMatchesDragGesture == null) Debug.LogError("[AltarGesture] closeMatchesDragGesture is null!");
            if (lightMatchesDragGesture == null) Debug.LogError("[AltarGesture] lightMatchesDragGesture is null!");

            openMatchesDragGesture.OnGestureEnd += OnOpenMatchGesture;
            closeMatchesDragGesture.OnGestureEnd += OnCloseMatchGesture;
            lightMatchesDragGesture.OnGestureEnd += OnLightMatchGesture;
        }

        private void OnDisable()
        {
            Debug.Log("[AltarGesture] OnDisable() — unsubscribing from gesture events.");
            openMatchesDragGesture.OnGestureEnd -= OnOpenMatchGesture;
            closeMatchesDragGesture.OnGestureEnd -= OnCloseMatchGesture;
            lightMatchesDragGesture.OnGestureEnd -= OnLightMatchGesture;
        }
        
        public void Awake()
        {
            _audio = AudioManager.Instance;
        }
        
        public void PlaceMatches()
        {
            Debug.Log($"[AltarGesture] PlaceMatches() called. Already placed: {_matchesPlaced}");
            if (_matchesPlaced) return;

            _matchesPlaced = true;
            if (matchObject != null) matchObject.SetActive(true);
            Debug.Log("[AltarGesture] Matches placed. matchObject active.");

            if (matchesConsumedStateKey != null)
            {
                Debug.Log($"[AltarGesture] Setting consumed state '{matchesConsumedStateKey.name}' to true.");
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Match.place, GameObject.Find("Character"));
                ServiceLocator.Instance.Get<GameStateManager>().SetState(matchesConsumedStateKey, true);
            }

            TryEnableGestures();
        }
        
        public void PlaceCandle()
        {
            Debug.Log($"[AltarGesture] PlaceCandle() called. Already placed: {_candlePlaced}");
            if (_candlePlaced) return;

            _candlePlaced = true;
            if (candleObject != null) candleObject.SetActive(true);
            Debug.Log("[AltarGesture] Candle placed. candleObject active.");

            if (candleConsumedStateKey != null)
            {
                Debug.Log($"[AltarGesture] Setting consumed state '{candleConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(candleConsumedStateKey, true);
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Candle.place, GameObject.Find("Character"));
            }

            TryEnableGestures();
        }

        public bool IsMatchesPlaced => _matchesPlaced;
        public bool IsCandlePlaced => _candlePlaced;
        public bool IsMatchesLit => _matchesLit;

        private void TryEnableGestures()
        {
            Debug.Log($"[AltarGesture] TryEnableGestures() — matchesPlaced={_matchesPlaced}, candlePlaced={_candlePlaced}");
            if (_matchesPlaced && _candlePlaced)
            {
                Debug.Log("[AltarGesture] Both items placed — enabling gestures.");
                SetGesturesEnabled(true);
            }
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            Debug.Log($"[AltarGesture] SetGesturesEnabled({isEnabled})");
            if (openMatchesDragGesture != null) openMatchesDragGesture.gameObject.SetActive(isEnabled);
            if (closeMatchesDragGesture != null) closeMatchesDragGesture.gameObject.SetActive(isEnabled);
            if (lightMatchesDragGesture != null) lightMatchesDragGesture.gameObject.SetActive(isEnabled);
        }

        private void OnLightMatchGesture(DragDirection dragDirection)
        {
            Debug.Log($"[AltarGesture] OnLightMatchGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Right)
                LightMatches();
        }

        private void OnCloseMatchGesture(DragDirection dragDirection)
        {
            Debug.Log($"[AltarGesture] OnCloseMatchGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Left)
                CloseMatches();
        }

        private void OnOpenMatchGesture(DragDirection dragDirection)
        {
            Debug.Log($"[AltarGesture] OnOpenMatchGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Right)
                SwapToOpenMatches();
        }

        private void LightMatches()
        {
            Debug.Log("[AltarGesture] LightMatches() — closing gesture UI and setting state.");
            _matchesLit = true;
            GestureHelper.CloseGestureUI(panel);

            if (matchesLitStateKey != null)
            {
                Debug.Log($"[AltarGesture] Setting GameState '{matchesLitStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(matchesLitStateKey, true);
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Match.light, GameObject.Find("Character"));
            }
            else
            {
                Debug.LogWarning("[AltarGesture] matchesLitStateKey is null — no game state set.");
            }

            UpdateWorldObjects();
        }

        private void CloseMatches()
        {
            Debug.Log("[AltarGesture] CloseMatches()");
            _matchesOpen = false;
            openMatches.SetActive(false);
            closedMatches.SetActive(true);
            _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Match.place, GameObject.Find("Character"));
        }


        private void SwapToOpenMatches()
        {
            Debug.Log("[AltarGesture] SwapToOpenMatches()");
            _matchesOpen = true;
            closedMatches.SetActive(false);
            openMatches.SetActive(true);
        }

        private void UpdateWorldObjects()
        {
            Debug.Log($"[AltarGesture] UpdateWorldObjects() — matchesOpen={_matchesOpen}, matchesLit={_matchesLit}");
            if (matchObject != null)
            {
                bool matchActive = _matchesOpen && !_matchesLit;
                matchObject.SetActive(matchActive);
                Debug.Log($"[AltarGesture] matchObject active={matchActive}");
            }

            if (candleObject != null)
            {
                candleObject.SetActive(_matchesLit);
                Debug.Log($"[AltarGesture] candleObject active={_matchesLit}");
            }
        }
    }
}
