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

        [Header("Game State")]
        public GameStateKey matchesLitStateKey;

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
            panel.SetActive(false);

            if (matchesLitStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(matchesLitStateKey, true);
            }
        }


        private void CloseMatches()
        {
            openMatches.SetActive(false);
            closedMatches.SetActive(true);
        }


        private void SwapToOpenMatches()
        {
            closedMatches.SetActive(false);
            openMatches.SetActive(true);
        }

    }
}