using Services;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace UI
{
    public class RitualAmuletPlace : MonoBehaviour
    {
        public GameObject panel;
        public GameObject amulet;
        
        private AudioManager audio;

        [Header("Game State")]
        public GameStateKey amuletPlaced;
        
        private void Awake()
        {
            audio = AudioManager.Instance;
        }

        private void setState()
        {
            Debug.Log("success!");
            panel.SetActive(false);

            if (amuletPlaced != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(amuletPlaced, true);
                audio.PlayOneShot(AudioID.SFX.Player.Interact.Amulet.place, GameObject.Find("Character"));
            }
        }

    }
}
