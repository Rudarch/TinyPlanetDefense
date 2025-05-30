using UnityEngine;

public class ShockwaveEffect : MonoBehaviour
{
    public float maxRadius = 1f;
    public float expandSpeed = 5f;
    public float fadeDuration = 0.2f;

    private SpriteRenderer sr;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float scale = Mathf.Min(maxRadius, timer * expandSpeed);
        float worldScale = scale * 2f; // Since default sprite diameter = 1, we need full diameter to match radius in Unity units

        transform.localScale = new Vector3(worldScale, worldScale, 1f);

        float alpha = Mathf.Lerp(0.5f, 0f, timer / fadeDuration);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (timer > fadeDuration)
            Destroy(gameObject);
    }
}
