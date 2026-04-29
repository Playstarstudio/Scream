using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{

    public class GameStateManager : IService
    {
        private readonly Dictionary<GameStateKey, bool> _states = new Dictionary<GameStateKey, bool>();
        
        public event Action<GameStateKey, bool> OnStateChanged;
        
        public bool GetState(GameStateKey key)
        {
            if (key == null)
            {
                Debug.LogError("[GameStateManager] Attempted to get state with a null key.");
                return false;
            }

            if (!_states.TryGetValue(key, out bool value))
            {
                value = key.defaultValue;
                _states[key] = value;
            }

            return value;
        }
        
        public void SetState(GameStateKey key, bool value)
        {
            if (key == null)
            {
                Debug.LogError("[GameStateManager] Attempted to set state with a null key.");
                return;
            }

            bool previous = GetState(key);
            _states[key] = value;
            key.currentValue = value;

            if (previous != value)
            {
                OnStateChanged?.Invoke(key, value);
            }
        }

        public bool ToggleState(GameStateKey key)
        {
            bool newValue = !GetState(key);
            SetState(key, newValue);
            return newValue;
        }


        public void ResetState(GameStateKey key)
        {
            if (key == null) return;
            SetState(key, key.defaultValue);
        }
        
        public void ResetAll()
        {
            // Collect keys first to avoid modifying the collection while iterating.
            var keys = new List<GameStateKey>(_states.Keys);
            foreach (var key in keys)
            {
                SetState(key, key.defaultValue);
            }
        }
    }
}
