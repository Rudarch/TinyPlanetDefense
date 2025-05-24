using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RuntimeRangeVisualizer : MonoBehaviour
{
    public Transform centerPoint;
    public AutoCannonController cannon;
    public int segments = 64;
    public Color color = Color.green;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        DrawCircle();
    }

    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        if (centerPoint == null || cannon == null) return;

        float radius = cannon.firingRange;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = centerPoint.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = centerPoint.position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }
}