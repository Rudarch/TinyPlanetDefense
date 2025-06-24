using UnityEngine;

public class MoltenExplosionFX : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float targetRadius = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timer = 0f;
    private Color startColor;

    void Start()
    {
        transform.localScale = Vector3.zero;
        startColor = spriteRenderer.color;

        // Ensure Z is 1 to avoid disappearing if scale is 0
        transform.localScale = new Vector3(0f, 0f, 1f);

        // Scale particle burst size
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var shape = ps.shape;
            shape.radius = targetRadius * 0.5f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Expand based on explosion radius
        float t = timer / fadeDuration;
        float currentRadius = Mathf.Lerp(0f, targetRadius, t);
        float worldScale = currentRadius * 2f;
        transform.localScale = new Vector3(worldScale, worldScale, 1f);

        // Fade out
        float alpha = Mathf.Lerp(1f, 0f, t);
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetRadius(float radius)
    {
        targetRadius = radius;
    }
}
