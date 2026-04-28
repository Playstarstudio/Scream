using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class BackpackInventoryScript : MonoBehaviour
    {
        public DragGesture dragGesture;
        public RectTransform inventory;
        
        private float _animationDuration = 0.4f;
     
        [Tooltip("The easing curve for the animation")]
        private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public Vector2 hiddenOffset;
        private Vector2 _closedPosition;
        private Vector2 _openPosition;
     
        private bool _isOpen = false;
        private bool _isAnimating = false;
        private Coroutine _activeCoroutine;

        private void OnEnable()
        {
            dragGesture.OnGestureEnd += OnGesture;
        }

        private void OnDisable()
        {
            dragGesture.OnGestureEnd -= OnGesture;
        }

        private void Awake()
        {
            _openPosition = inventory.anchoredPosition;
            _closedPosition = _openPosition + hiddenOffset;

            inventory.anchoredPosition = _closedPosition;
            inventory.gameObject.SetActive(false);
        }

        private void OnGesture(DragDirection dragDirection)
        {
            Debug.Log("OnGesture: " + dragDirection);
            if (_isAnimating) return;
     
            if (_isOpen && dragDirection == DragDirection.Down)
                SlideDown();
            else if (dragDirection == DragDirection.Up)
                SlideUp();
        }

        private void SlideUp()
        {
            _isOpen = true;
            inventory.gameObject.SetActive(true);
            if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            _activeCoroutine = StartCoroutine(AnimateBackpack(_closedPosition, _openPosition));
        }

        private void SlideDown()
        {
            _isOpen = false;
            if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            _activeCoroutine = StartCoroutine(AnimateBackpack(_openPosition, _closedPosition));
        }

        private IEnumerator AnimateBackpack(Vector2 from, Vector2 to)
        {
            _isAnimating = true;
            var elapsed = 0f;
     
            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / _animationDuration);
                var curvedT = _easeCurve.Evaluate(t);
                inventory.anchoredPosition = Vector2.LerpUnclamped(from, to, curvedT);
                yield return null;
            }

            inventory.anchoredPosition = to;
            if (!_isOpen)
            {
                inventory.gameObject.SetActive(false);
            }
            _isAnimating = false;
        }
    }
}