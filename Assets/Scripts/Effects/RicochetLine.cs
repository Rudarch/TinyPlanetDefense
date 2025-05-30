using UnityEngine;

public class RicochetLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float fadeDuration = 0.3f;

    private float timer = 0f;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > fadeDuration)
        {
            Destroy(gameObject);
        }
        else
        {
            float alpha = 1f - (timer / fadeDuration);
            Color c = lineRenderer.startColor;
            c.a = alpha;
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
        }
    }

    public void SetPoints(Vector3 from, Vector3 to)
    {
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }
}
