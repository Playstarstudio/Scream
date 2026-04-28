using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UI
{
    public delegate void OnGestureEnd(DragDirection dragDirection);
    
    [System.Flags]
    public enum DragDirection
    {
        Nothing = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Everything = -1
    }
    
    public class DragGesture : MonoBehaviour, 
        IPointerDownHandler, 
        IPointerUpHandler, 
        IDragHandler
    {
        public event  OnGestureEnd OnGestureEnd;
        [SerializeField] private bool moveTransformWithGesture;
        [SerializeField] private bool persistTransformWithGesture;
        [SerializeField] private float dragDistance = 100f;
        [SerializeField] private DragDirection enabledDragDirections = DragDirection.Everything;
        [SerializeField] private DragDirection startingDragDirection = DragDirection.Nothing;

        private Vector2 _dragStart;
        private DragDirection _lastSuccessfulDragDirection = DragDirection.Nothing;
        private Vector2 _lastAnchoredPosition;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(_rectTransform, "DragGesture must be attached to a RectTransform");
            _lastAnchoredPosition = _rectTransform.anchoredPosition;
            _lastSuccessfulDragDirection = startingDragDirection;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            _dragStart = eventData.position;
            Debug.Log("Drag Start: " + _dragStart);
        }
        
        // "tick" of drag interaction
        public void OnDrag(PointerEventData eventData)
        {
             /* keeps drag alive */
             var delta = eventData.position - _dragStart;
             Debug.Log(delta);
             if (moveTransformWithGesture)
             {
                 var clampedDelta = Vector2.zero;
                 if (enabledDragDirections.HasFlag(DragDirection.Up) ||
                     enabledDragDirections.HasFlag(DragDirection.Down))
                 {
                     clampedDelta.y = Mathf.Clamp(delta.y,
                         (_lastSuccessfulDragDirection == DragDirection.Down) ? 0.0f : -dragDistance,
                         (_lastSuccessfulDragDirection == DragDirection.Up) ? 0.0f : dragDistance);
                 }

                 if (enabledDragDirections.HasFlag(DragDirection.Left) ||
                     enabledDragDirections.HasFlag(DragDirection.Right))
                 {
                     clampedDelta.x = Mathf.Clamp(delta.x,
                         (_lastSuccessfulDragDirection == DragDirection.Left) ? 0.0f : -dragDistance,
                         (_lastSuccessfulDragDirection == DragDirection.Right) ? 0.0f : dragDistance);
                 }
                 _rectTransform.anchoredPosition = _lastAnchoredPosition + clampedDelta;
                 Debug.Log("Tick Last anchored position:" + _lastAnchoredPosition);
                 Debug.Log("Tick Real anchored position:" + _rectTransform.anchoredPosition);
             }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");

            var delta = eventData.position - _dragStart;

            var dragDirection = DetectDragDirection(delta);
            if (delta.magnitude >= dragDistance && dragDirection != _lastSuccessfulDragDirection)
            {
                // SUCCESSFUL GESTURE
                OnGestureEnd?.Invoke(dragDirection);
                if (persistTransformWithGesture)
                {
                    _lastAnchoredPosition = _rectTransform.anchoredPosition; // snap to new anchored position
                    _lastSuccessfulDragDirection = dragDirection;
                }
                else
                {
                    _rectTransform.anchoredPosition = _lastAnchoredPosition;
                }
            }
            else
            {
                // didn't drag far enough, snap back to previous position
                OnGestureEnd?.Invoke(DragDirection.Nothing);
                _rectTransform.anchoredPosition = _lastAnchoredPosition;
            }
        }

        private DragDirection DetectDragDirection(Vector2 delta)
        {
            if (enabledDragDirections.HasFlag(DragDirection.Up) && delta.y > dragDistance)
            {
                return DragDirection.Up;
            }
            else if (enabledDragDirections.HasFlag(DragDirection.Down) && delta.y < -dragDistance)
            {
                return DragDirection.Down;
            }
            else if (enabledDragDirections.HasFlag(DragDirection.Left) && delta.x < -dragDistance)
            {
                return DragDirection.Left;
            }
            else if (enabledDragDirections.HasFlag(DragDirection.Right) && delta.x > dragDistance)
            {
                return DragDirection.Right;
            }

            return DragDirection.Nothing;
        }
    }
}