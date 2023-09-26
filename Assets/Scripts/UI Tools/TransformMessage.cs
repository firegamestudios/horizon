using UnityEngine;

public class TransformMessage : MonoBehaviour
{
    public Transform transformInWorld;
    private RectTransform rectTransform;
    private Camera mainCamera;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void SetupTransform(Transform _trans)
    {
        transformInWorld = _trans;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (transformInWorld == null || mainCamera == null) return;

        // Convert world position to screen position
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(transformInWorld.position);

        // Set the position
        rectTransform.position = screenPosition;
    }

    private void Update()
    {
        // If you want the UI element to follow the 3D object even if it moves, 
        // you can update its position every frame.
        UpdatePosition();
    }
}
