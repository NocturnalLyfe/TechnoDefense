using UnityEngine;

public class Draggable : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float springForce = 50f;
    [SerializeField] private float dragCoefficient = 5f;
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float maxDragSpeed = 15f;
    [SerializeField] private float releaseDrag = 2f;

    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 targetPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"Draggable on {gameObject.name} requires a Rigidbody2D component!");
        }

        // Set initial drag
        rb.linearDamping = releaseDrag;
    }

    void FixedUpdate()
    {
        if (isDragging && rb != null)
        {
            ApplyDragForce();
        }
    }

    // Called by DragManager when drag starts
    public void StartDrag(Vector2 mousePos)
    {
        isDragging = true;
        targetPosition = mousePos;
        rb.linearDamping = dragCoefficient;
    }

    // Called by DragManager every frame while dragging
    public void UpdateDrag(Vector2 mousePos)
    {
        targetPosition = mousePos;
    }

    // Called by DragManager when drag ends
    public void EndDrag()
    {
        isDragging = false;
        rb.linearDamping = releaseDrag;
    }

    private void ApplyDragForce()
    {
        Vector2 directionToTarget = targetPosition - (Vector2)transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > stopThreshold)
        {
            // Spring force proportional to distance
            Vector2 force = directionToTarget.normalized * springForce * distanceToTarget;
            rb.AddForce(force);

            // Clamp velocity
            if (rb.linearVelocity.magnitude > maxDragSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxDragSpeed;
            }
        }
        else
        {
            // Dampen when close
            rb.linearVelocity *= 0.9f;
        }
    }

    public bool IsDragging() => isDragging;

    // Optional: Visual feedback
    void OnDrawGizmos()
    {
        if (isDragging && Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPosition, stopThreshold);
        }
    }
}