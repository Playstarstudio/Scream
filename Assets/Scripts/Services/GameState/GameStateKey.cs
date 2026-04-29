using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "NewGameStateKey", menuName = "Game State/Key")]
    public class GameStateKey : ScriptableObject
    {
        [Tooltip("The default value of this flag when the game starts.")]
        public bool defaultValue;
        
        [HideInInspector]
        public bool currentValue;

        private void OnEnable()
        {
            currentValue = defaultValue;
        }
    }
}
