using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraZoomAndPan : MonoBehaviour
{
    public float zoomSpeedTouch = 0.01f;
    public float zoomSpeedScroll = 1f;
    public float panSpeed = 0.01f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private Camera cam;
    private Vector3 lastMousePos;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMouseZoom();
        HandleMousePan();
        HandleTouchZoom();
        HandleTouchPan();
    }

    void HandleMouseZoom()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeedScroll;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleMousePan()
    {
        if (Mouse.current == null || !Mouse.current.rightButton.isPressed) return;

        Vector3 mouseDelta = Mouse.current.delta.ReadValue();
        Vector3 move = new Vector3(-mouseDelta.x, -mouseDelta.y, 0f) * panSpeed;
        transform.position += cam.ScreenToWorldPoint(move) - cam.ScreenToWorldPoint(Vector3.zero);
    }

    void HandleTouchZoom()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2) return;

        var t0 = Touchscreen.current.touches[0];
        var t1 = Touchscreen.current.touches[1];

        if (!t0.isInProgress || !t1.isInProgress) return;

        Vector2 prevT0 = t0.position.ReadValue() - t0.delta.ReadValue();
        Vector2 prevT1 = t1.position.ReadValue() - t1.delta.ReadValue();

        float prevDist = Vector2.Distance(prevT0, prevT1);
        float currDist = Vector2.Distance(t0.position.ReadValue(), t1.position.ReadValue());
        float delta = prevDist - currDist;

        cam.orthographicSize += delta * zoomSpeedTouch;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    void HandleTouchPan()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count != 2) return;

        Vector2 delta0 = Touchscreen.current.touches[0].delta.ReadValue();
        Vector2 delta1 = Touchscreen.current.touches[1].delta.ReadValue();

        Vector2 avgDelta = (delta0 + delta1) * 0.5f;
        Vector3 move = new Vector3(-avgDelta.x, -avgDelta.y, 0f) * panSpeed;

        transform.position += cam.ScreenToWorldPoint(move) - cam.ScreenToWorldPoint(Vector3.zero);
    }
}
