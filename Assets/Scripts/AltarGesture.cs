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

        private bool _matchesPlaced;
        private bool _candlePlaced;
        private bool _matchesOpen;
        private bool _matchesLit;

        private void Start()
        {
            // Both items start disabled until placed from inventory
            if (matchObject != null) matchObject.SetActive(false);
            if (candleObject != null) candleObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            openMatchesDragGesture.OnGestureEnd += OnOpenMatchGesture;
            closeMatchesDragGesture.OnGestureEnd += OnCloseMatchGesture;
            lightMatchesDragGesture.OnGestureEnd += OnLightMatchGesture;
        }

        private void OnDisable()
        {
            openMatchesDragGesture.OnGestureEnd -= OnOpenMatchGesture;
            closeMatchesDragGesture.OnGestureEnd -= OnCloseMatchGesture;
            lightMatchesDragGesture.OnGestureEnd -= OnLightMatchGesture;
        }
        
        public void PlaceMatches()
        {
            if (_matchesPlaced) return;

            _matchesPlaced = true;
            if (matchObject != null) matchObject.SetActive(true);

            TryEnableGestures();
        }
        
        public void PlaceCandle()
        {
            if (_candlePlaced) return;

            _candlePlaced = true;
            if (candleObject != null) candleObject.SetActive(true);

            TryEnableGestures();
        }

        public bool IsMatchesPlaced => _matchesPlaced;
        public bool IsCandlePlaced => _candlePlaced;
        public bool IsMatchesLit => _matchesLit;

        private void TryEnableGestures()
        {
            if (_matchesPlaced && _candlePlaced)
            {
                SetGesturesEnabled(true);
            }
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            if (openMatchesDragGesture != null) openMatchesDragGesture.gameObject.SetActive(isEnabled);
            if (closeMatchesDragGesture != null) closeMatchesDragGesture.gameObject.SetActive(isEnabled);
            if (lightMatchesDragGesture != null) lightMatchesDragGesture.gameObject.SetActive(isEnabled);
        }

        private void OnLightMatchGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Right)
                LightMatches();
        }

        private void OnCloseMatchGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Left)
                CloseMatches();
        }

        private void OnOpenMatchGesture(DragDirection dragDirection)
        {
            Debug.Log("onlight");
            if (dragDirection == DragDirection.Right)
                SwapToOpenMatches();
        }

        private void LightMatches()
        {
            Debug.Log("success!");
            _matchesLit = true;
            panel.SetActive(false);

            if (matchesLitStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(matchesLitStateKey, true);
            }

            UpdateWorldObjects();
        }

        private void CloseMatches()
        {
            _matchesOpen = false;
            openMatches.SetActive(false);
            closedMatches.SetActive(true);
        }


        private void SwapToOpenMatches()
        {
            _matchesOpen = true;
            closedMatches.SetActive(false);
            openMatches.SetActive(true);
        }

        private void UpdateWorldObjects()
        {
            if (matchObject != null)
            {
                matchObject.SetActive(_matchesOpen && !_matchesLit);
            }

            if (candleObject != null)
            {
                candleObject.SetActive(_matchesLit);
            }
        }
    }
}