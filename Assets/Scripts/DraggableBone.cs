using UnityEngine;
using UnityEngine.InputSystem; // Required for New Input System

public class DraggableBone : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    public GameObject dog; // Assign your dog GameObject in Inspector
    public float feedDistance = 2f; // How close to drop the bone on dog

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // 1. Detect Initial Click (Replacement for OnMouseDown)
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition(mouse.position.ReadValue());

            // Perform a 2D overlap check to see if we clicked THIS bone
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;
            }
        }

        // 2. Handle Dragging
        if (isDragging)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition(mouse.position.ReadValue());
            transform.position = mouseWorldPos + offset;

            // 3. Detect Release (Replacement for OnMouseUp)
            if (mouse.leftButton.wasReleasedThisFrame)
            {
                StopDragging();
            }
        }
    }

    void StopDragging()
    {
        isDragging = false;

        if (dog != null)
        {
            float distanceToDog = Vector3.Distance(transform.position, dog.transform.position);

            if (distanceToDog < feedDistance)
            {
                FeedDog();
            }
            else
            {
                ResetBone();
            }
        }
    }

    void FeedDog()
    {
        Debug.Log("Dog fed!");

        // Find the health bar script in your scene
        HealthBarCanvas healthSystem = Object.FindFirstObjectByType<HealthBarCanvas>();

        if (healthSystem != null)
        {
            healthSystem.AddHealth(20f); // Increase health by 20 points
        }
        else
        {
            Debug.LogWarning("Could not find HealthBarCanvas in the scene!");
        }

        // Hide the bone after eating
        gameObject.SetActive(false);
    }

    void ResetBone()
    {
        // Move back to a start position or hide
        gameObject.SetActive(false);
    }

    // Updated to take the screen position as a parameter
    Vector3 GetMouseWorldPosition(Vector2 screenPosition)
    {
        Vector3 mousePoint = new Vector3(screenPosition.x, screenPosition.y, 0);
        // Ensure the Z distance matches the object's distance from camera
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}