using Services;
using UnityEngine;

namespace UI
{


    public class SmearTeddy : MonoBehaviour
    {
        public DragGesture TeddyDrag;
        public GameObject panel;
        public GameObject teddy;
        
        private new AudioManager audio;

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
        
        private void Awake()
        {
            audio = AudioManager.Instance;
        }

        private void OnTeddyDragGesture(DragDirection dragDirection)
        {
            if (dragDirection == DragDirection.Right)
                setState();
        }
        private void setState()
        {
            Debug.Log("success!");
            GestureHelper.CloseGestureUI(panel);

            if (teddyDraggedStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(teddyDraggedStateKey, true);
                audio.PlayOneShot(AudioID.SFX.Player.Interact.Teddy_Bear.smear, GameObject.Find("Character"));
            }
        }
    }
}
