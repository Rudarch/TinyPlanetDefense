using UnityEngine;

public class DynamicCameraZoom : MonoBehaviour
{
    public float defaultSize = 5f;
    public float zoomOutMargin = 1f;
    public float zoomSpeed = 2f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        if (enemies.Length == 0)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize, Time.deltaTime * zoomSpeed);
            return;
        }

        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Enemy e in enemies)
        {
            if (e != null && e.gameObject.activeInHierarchy)
                bounds.Encapsulate(e.transform.position);
        }

        float verticalExtent = bounds.extents.y + zoomOutMargin;
        float horizontalExtent = (bounds.extents.x + zoomOutMargin) / cam.aspect;

        float requiredSize = Mathf.Max(verticalExtent, horizontalExtent, defaultSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, requiredSize, Time.deltaTime * zoomSpeed);
    }
}
