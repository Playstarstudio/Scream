using Services;
using UnityEngine;

namespace UI
{


    public class SmearTeddy : MonoBehaviour
    {
        public DragGesture TeddyDrag;
        public GameObject panel;
        public GameObject teddy;

        [Header("Game State")]
        public GameStateKey teddyDraggedStateKey;

        private void OnEnable()
        {
            TeddyDrag.OnGestureEnd += OnTeddyDragGesture;
        }


        private void OnDisable()
        {
            TeddyDrag.OnGestureEnd -= OnTeddyDragGesture;
        }

        private void OnTeddyDragGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Right)
                setState();
        }
        private void setState()
        {
            Debug.Log("success!");
            panel.SetActive(false);

            if (teddyDraggedStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyDraggedStateKey, true);
            }
        }
    }
}