using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class LanternWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("How much padding to add around the screen to restrict the draggable area")]
    public Vector2 screenPadding = Vector2.zero;

    private RectTransform _rect;
    private RectTransform _parentRect;
    private Canvas _canvas;
    private Vector2 _dragOffset;
    private AudioManager _audio;

    private void Awake()
    {
        _rect       = GetComponent<RectTransform>();
        _parentRect = transform.parent as RectTransform;
        _canvas     = GetComponentInParent<Canvas>();

        if (_canvas == null)
            Debug.LogWarning($"[DraggableHUDWidget] No Canvas found above {name}. Dragging won't work.");
            
        _audio = AudioManager.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRect,
            eventData.position,
            GetEventCamera(eventData),
            out Vector2 localPointer
        );

        _dragOffset = _rect.anchoredPosition - localPointer;
        
        _audio.PlayOneShot(AudioID.SFX.Interface.Inventory.select);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _audio.PlayOneShot(AudioID.SFX.Interface.Inventory.unselect);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_parentRect == null || _canvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRect,
            eventData.position,
            GetEventCamera(eventData),
            out Vector2 localPointer
        );

        var targetPos = localPointer + _dragOffset;
        _rect.anchoredPosition = targetPos;
        ClampToScreen();
    }
    
    private void ClampToScreen()
    {
        var worldCorners = new Vector3[4];
        _rect.GetWorldCorners(worldCorners); // Bottom-left, Top-left, Top-right, Bottom-right

        var parentRect = _rect.parent as RectTransform;
        var parentCorners = new Vector3[4];
        if (parentRect == null)
        {
            return;
        }
            
        parentRect.GetWorldCorners(parentCorners);

        Vector3 currentPos = _rect.anchoredPosition;

        // Calculate how much the element is "overflowing" its parent
        var minX = (parentCorners[0].x + (currentPos.x - worldCorners[0].x)) + 32.0f;
        var maxX = (parentCorners[2].x - (worldCorners[2].x - currentPos.x)) - 32.0f;
        var minY = (parentCorners[0].y + (currentPos.y - worldCorners[0].y)) + 32.0f;
        var maxY = (parentCorners[2].y - (worldCorners[2].y - currentPos.y)) - 32.0f;

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        _rect.anchoredPosition = currentPos;
    }

    private Camera GetEventCamera(PointerEventData eventData)
    {
        return _canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : _canvas.worldCamera;
    }
}
