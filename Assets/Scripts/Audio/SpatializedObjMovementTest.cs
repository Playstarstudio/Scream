using UnityEngine;

public class SpatializedObjMovementTest : MonoBehaviour
{
    public Vector2 velocity = new Vector2(3f, 3f);

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteExtents;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpatializationTest requires a SpriteRenderer component.");
            enabled = false;
            return;
        }

        spriteExtents = spriteRenderer.bounds.extents;
    }

    void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);

        Vector3 pos = transform.position;

        // Get screen bounds in world coordinates
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z * -1)
        );

        // Bounce horizontally
        if (pos.x + spriteExtents.x > screenBounds.x || pos.x - spriteExtents.x < -screenBounds.x)
        {
            velocity.x *= -1;
            pos.x = Mathf.Clamp(pos.x, -screenBounds.x + spriteExtents.x, screenBounds.x - spriteExtents.x);
        }

        // Bounce vertically
        if (pos.y + spriteExtents.y > screenBounds.y || pos.y - spriteExtents.y < -screenBounds.y)
        {
            velocity.y *= -1;
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y + spriteExtents.y, screenBounds.y - spriteExtents.y);
        }

        transform.position = pos;
    }
}
