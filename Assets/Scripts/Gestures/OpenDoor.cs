using Services;
using UnityEngine;

namespace UI
{
    public class OpenDoor : MonoBehaviour
    {
        public DragGesture doorOpenGesture;
        public GameObject panel;

        [Header("World Objects")]
        [Tooltip("The key object in the world to show/hide based on placement state.")]
        public GameObject keyObject;

        [Header("Game State")]
        public GameStateKey doorOpenedStateKey;

        [Header("Consumed States")]
        [Tooltip("Set to true when the key is placed. Assign the same key to the key KeyItem's consumedStateKey.")]
        public GameStateKey keyConsumedStateKey;
        

        private AudioManager _audio;
        private bool _keyPlaced;
        private bool _doorOpened;

        public bool IsKeyPlaced => _keyPlaced;
        public bool IsDoorOpened => _doorOpened;

        private void Awake()
        {
            _audio = AudioManager.Instance;
        }

        private void Start()
        {
            Debug.Log($"[OpenDoor] Start() — panel={panel?.name ?? "null"}, " +
                      $"keyObject={keyObject?.name ?? "null"}, " +
                      $"gesture={doorOpenGesture?.name ?? "null"}");

            if (keyObject != null) keyObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            Debug.Log("[OpenDoor] OnEnable() — subscribing to gesture events.");
            if (doorOpenGesture == null) Debug.LogError("[OpenDoor] doorOpenGesture is null!");
            doorOpenGesture.OnGestureEnd += OnDoorOpenGesture;
        }

        private void OnDisable()
        {
            Debug.Log("[OpenDoor] OnDisable() — unsubscribing from gesture events.");
            doorOpenGesture.OnGestureEnd -= OnDoorOpenGesture;
        }

        public void PlaceKey()
        {
            Debug.Log($"[OpenDoor] PlaceKey() called. Already placed: {_keyPlaced}");
            if (_keyPlaced) return;

            _keyPlaced = true;
            if (keyObject != null) keyObject.SetActive(true);
            Debug.Log("[OpenDoor] Key placed. keyObject active.");

            if (keyConsumedStateKey != null)
            {
                Debug.Log($"[OpenDoor] Setting consumed state '{keyConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(keyConsumedStateKey, true);
            }

            Debug.Log("[OpenDoor] Key placed — enabling gestures.");
            SetGesturesEnabled(true);
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            Debug.Log($"[OpenDoor] SetGesturesEnabled({isEnabled})");
            if (doorOpenGesture != null) doorOpenGesture.gameObject.SetActive(isEnabled);
        }

        private void OnDoorOpenGesture(DragDirection dragDirection)
        {
            Debug.Log($"[OpenDoor] OnDoorOpenGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Right || dragDirection == DragDirection.Left)
                OpenTheDoor();
        }

        private void OpenTheDoor()
        {
            Debug.Log("[OpenDoor] OpenTheDoor() — closing gesture UI and setting state.");
            _doorOpened = true;
            GestureHelper.CloseGestureUI(panel);

            if (doorOpenedStateKey != null)
            {
                Debug.Log($"[OpenDoor] Setting GameState '{doorOpenedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(doorOpenedStateKey, true);
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Door.open, GameObject.Find("Character"));
            }
            else
            {
                Debug.LogWarning("[OpenDoor] doorOpenedStateKey is null — no game state set.");
            }
        }
    }
}
