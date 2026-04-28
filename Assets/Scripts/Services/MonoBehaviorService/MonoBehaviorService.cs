using System;
using UnityEngine;

namespace Services
{
    public class MonoBehaviorService : MonoBehaviour, IService
    {
        public event Action AwakeEvent;
        public event Action StartEvent;
        public event Action UpdateEvent;
        public event Action LateUpdateEvent;
        public event Action FixedUpdateEvent;
        private void Awake()
        {
            AwakeEvent?.Invoke();
        }

        private void Start()
        {
            StartEvent?.Invoke();
        }

        private void Update()
        {
            UpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }
        
    }
}
