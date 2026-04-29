using Services;
using UnityEngine;

namespace UI
{


    public class OpenDoor : MonoBehaviour
    {
        public DragGesture doorOpenGesture;
        public GameObject panel;
        public GameObject key;
        public GameObject door;

        [Header("Game State")]
        public GameStateKey doorOpenedStateKey;

        private void OnEnable()
        {
            doorOpenGesture.OnGestureEnd += OnDoorOpenGesture;
        }


        private void OnDisable()
        {
            doorOpenGesture.OnGestureEnd -= OnDoorOpenGesture;
        }

        private void OnDoorOpenGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Down)
                setState();
        }
        private void setState()
        {
            Debug.Log("success!");
            panel.SetActive(false);

            if (doorOpenedStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(doorOpenedStateKey, true);
            }
        }
    }
}