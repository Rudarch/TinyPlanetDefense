using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 0.2f;

    private Vector2 dragDelta;
    private bool isDragging = false;

    private void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Drag"].started += OnDragStarted;
            playerInput.actions["Drag"].performed += OnDragPerformed;
            playerInput.actions["Drag"].canceled += OnDragCanceled;
        }
    }

    private void OnDisable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Drag"].started -= OnDragStarted;
            playerInput.actions["Drag"].performed -= OnDragPerformed;
            playerInput.actions["Drag"].canceled -= OnDragCanceled;
        }
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        isDragging = true;
    }

    private void OnDragPerformed(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            dragDelta = context.ReadValue<Vector2>();
            transform.RotateAround(transform.parent.position, Vector3.forward, dragDelta.x * rotationSpeed);
        }
    }

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
        dragDelta = Vector2.zero;
    }
}