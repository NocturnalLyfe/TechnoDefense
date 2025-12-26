using UnityEngine;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{
    private Camera mainCamera;
    private Mouse mouse;
    private Draggable currentDragging = null;

    public enum DetectionMethod
    {
        Component,  // Check for Draggable component (recommended)
        Tag,        // Check for specific tag
        Layer       // Check for specific layer
    }

    void Start()
    {
        mainCamera = Camera.main;
        mouse = Mouse.current;

        if (mouse == null)
        {
            Debug.LogError("No mouse detected!");
        }
    }

    void Update()
    {
        if (mouse == null) return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouse.position.ReadValue());

        // Handle mouse press - start dragging
        if (mouse.leftButton.wasPressedThisFrame)
        {
            TryStartDrag(mouseWorldPos);
        }

        // Handle dragging - update position
        if (currentDragging != null && mouse.leftButton.isPressed)
        {
            currentDragging.UpdateDrag(mouseWorldPos);
        }

        // Handle mouse release - stop dragging
        if (mouse.leftButton.wasReleasedThisFrame && currentDragging != null)
        {
            currentDragging.EndDrag();
            currentDragging = null;
        }
    }

    private void TryStartDrag(Vector2 mouseWorldPos)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            // Try to get Draggable component
            Draggable draggable = hit.collider.GetComponent<Draggable>();

            if (draggable != null)
            {
                currentDragging = draggable;
                currentDragging.StartDrag(mouseWorldPos);
            }
        }
    }

    // Optional: Get currently dragged object
    public Draggable GetCurrentDragging() => currentDragging;

    // Optional: Force stop all dragging
    public void StopAllDragging()
    {
        if (currentDragging != null)
        {
            currentDragging.EndDrag();
            currentDragging = null;
        }
    }
}