using Services;
using UnityEngine;

namespace UI
{


    public class KillTeddy : MonoBehaviour
    {
        public DragGesture knifeDragGesture;
        public GameObject panel;
        public GameObject teddy;
        public GameObject knife;

        [Header("Game State")]
        public GameStateKey teddyKilledStateKey;

        private void OnEnable()
        {
            knifeDragGesture.OnGestureEnd += OnKnifeGesture;
        }


        private void OnDisable()
        {
            knifeDragGesture.OnGestureEnd -= OnKnifeGesture;
        }

        private void OnKnifeGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Down)
                setState();
        }
        private void setState()
        {
            Debug.Log("success!");
            panel.SetActive(false);

            if (teddyKilledStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyKilledStateKey, true);
            }
        }
    }
}