using Services;
using UnityEngine;

namespace UI
{
    public class KillTeddy : MonoBehaviour
    {
        public DragGesture knifeDragGesture;
        public GameObject panel;

        [Header("World Objects")]
        [Tooltip("The teddy object in the world to show/hide based on placement state.")]
        public GameObject teddyObject;
        [Tooltip("The knife object in the world to show/hide based on placement state.")]
        public GameObject knifeObject;

        [Header("Game State")]
        public GameStateKey teddyKilledStateKey;

        [Header("Consumed States")]
        [Tooltip("Set to true when the teddy is placed. Assign the same key to the teddy KeyItem's consumedStateKey.")]
        public GameStateKey teddyConsumedStateKey;
        [Tooltip("Set to true when the knife is placed. Assign the same key to the knife KeyItem's consumedStateKey.")]
        public GameStateKey knifeConsumedStateKey;

        [Header("Reward")]
        [Tooltip("The prefab to spawn when the teddy is killed.")]
        public GameObject teddySpawnPrefab;
        [Tooltip("Where to spawn the reward. If not set, spawns at this object's position.")]
        public Transform teddySpawnPoint;

        private AudioManager _audio;
        private bool _teddyPlaced;
        private bool _knifePlaced;
        private bool _teddyKilled;

        public bool IsTeddyPlaced => _teddyPlaced;
        public bool IsKnifePlaced => _knifePlaced;
        public bool IsTeddyKilled => _teddyKilled;

        private void Awake()
        {
            _audio = AudioManager.Instance;
        }

        private void Start()
        {
            Debug.Log($"[KillTeddy] Start() — panel={panel?.name ?? "null"}, " +
                      $"teddyObject={teddyObject?.name ?? "null"}, knifeObject={knifeObject?.name ?? "null"}, " +
                      $"gesture={knifeDragGesture?.name ?? "null"}");

            if (teddyObject != null) teddyObject.SetActive(false);
            if (knifeObject != null) knifeObject.SetActive(false);

            SetGesturesEnabled(false);
        }

        private void OnEnable()
        {
            Debug.Log("[KillTeddy] OnEnable() — subscribing to gesture events.");
            if (knifeDragGesture == null) Debug.LogError("[KillTeddy] knifeDragGesture is null!");
            knifeDragGesture.OnGestureEnd += OnKnifeGesture;
        }

        private void OnDisable()
        {
            Debug.Log("[KillTeddy] OnDisable() — unsubscribing from gesture events.");
            knifeDragGesture.OnGestureEnd -= OnKnifeGesture;
        }

        public void PlaceTeddy()
        {
            Debug.Log($"[KillTeddy] PlaceTeddy() called. Already placed: {_teddyPlaced}");
            if (_teddyPlaced) return;

            _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Teddy_Bear.place, GameObject.Find("Character"));
            _teddyPlaced = true;
            if (teddyObject != null) teddyObject.SetActive(true);
            Debug.Log("[KillTeddy] Teddy placed. teddyObject active.");

            if (teddyConsumedStateKey != null)
            {
                Debug.Log($"[KillTeddy] Setting consumed state '{teddyConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyConsumedStateKey, true);
            }

            TryEnableGestures();
        }

        public void PlaceKnife()
        {
            Debug.Log($"[KillTeddy] PlaceKnife() called. Already placed: {_knifePlaced}");
            if (_knifePlaced) return;

            _knifePlaced = true;
            if (knifeObject != null) knifeObject.SetActive(true);
            Debug.Log("[KillTeddy] Knife placed. knifeObject active.");

            if (knifeConsumedStateKey != null)
            {
                Debug.Log($"[KillTeddy] Setting consumed state '{knifeConsumedStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(knifeConsumedStateKey, true);
            }

            TryEnableGestures();
        }

        private void TryEnableGestures()
        {
            Debug.Log($"[KillTeddy] TryEnableGestures() — teddyPlaced={_teddyPlaced}, knifePlaced={_knifePlaced}");
            if (_teddyPlaced && _knifePlaced)
            {
                Debug.Log("[KillTeddy] Both items placed — enabling gestures.");
                SetGesturesEnabled(true);
            }
        }

        private void SetGesturesEnabled(bool isEnabled)
        {
            Debug.Log($"[KillTeddy] SetGesturesEnabled({isEnabled})");
            if (knifeDragGesture != null) knifeDragGesture.gameObject.SetActive(isEnabled);
        }

        private void OnKnifeGesture(DragDirection dragDirection)
        {
            Debug.Log($"[KillTeddy] OnKnifeGesture — direction={dragDirection}");
            if (dragDirection == DragDirection.Down)
                KillTheTeddy();
        }

        private void KillTheTeddy()
        {
            Debug.Log("[KillTeddy] KillTheTeddy() — closing gesture UI and setting state.");
            _teddyKilled = true;
            GestureHelper.CloseGestureUI(panel);

            SpawnReward();

            if (teddyKilledStateKey != null)
            {
                Debug.Log($"[KillTeddy] Setting GameState '{teddyKilledStateKey.name}' to true.");
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyKilledStateKey, true);
                _audio?.PlayOneShot(AudioID.SFX.Player.Interact.Teddy_Bear.slice, GameObject.Find("Character"));
            }
            else
            {
                Debug.LogWarning("[KillTeddy] teddyKilledStateKey is null — no game state set.");
            }
        }

        private void SpawnReward()
        {
            if (teddySpawnPrefab == null)
            {
                Debug.LogWarning("[KillTeddy] No teddySpawnPrefab assigned — nothing to spawn.");
                return;
            }
            /*
            Vector3 spawnPos = teddySpawnPoint != null ? teddySpawnPoint.position : transform.position;
            Quaternion spawnRot = teddySpawnPoint != null ? teddySpawnPoint.rotation : Quaternion.identity;
             */
            Vector3 spawnPos = FindFirstObjectByType<CharacterMovement>().transform.position;
            Quaternion spawnRot = FindFirstObjectByType<CharacterMovement>().transform.rotation;
            GameObject reward = Instantiate(teddySpawnPrefab, spawnPos, spawnRot);
            Debug.Log($"[KillTeddy] Spawned reward '{reward.name}' at {spawnPos}");
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 pos = teddySpawnPoint != null ? teddySpawnPoint.position : transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pos, 0.3f);
            Gizmos.DrawIcon(pos, "d_Prefab Icon", true);

            if (teddySpawnPoint != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, teddySpawnPoint.position);
            }
        }
    }
}
