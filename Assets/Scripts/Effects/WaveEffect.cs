using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    public float fadeDuration = 0.4f;
    public Sprite waveImage;
    public Color effectColor = Color.white;
    public float maxRadius = 5f;

    private SpriteRenderer sr;
    private float timer = 0f;
    private float expandSpeed;

    public void UpdateVisuals(Sprite waveImageReplacement, Color color, float radius, float fadeDuration)
    {
        if (waveImageReplacement != null)
        {
            UpdateVisuals(waveImageReplacement, color, radius);
            this.fadeDuration = fadeDuration;
        }
    }

    public void UpdateVisuals(Sprite waveImageReplacement, Color color, float radius)
    {
        if (waveImageReplacement != null)
        {
            waveImage = waveImageReplacement;
            maxRadius = radius;
            effectColor = color;
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        if (sr != null)
            sr.color = effectColor;

        if (sr != null && waveImage != null)
        {
            sr.sprite = waveImage;
        }
        expandSpeed = maxRadius / fadeDuration;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float scale = Mathf.Min(maxRadius, timer * expandSpeed);
        float worldScale = scale * 2f;
        transform.localScale = new Vector3(worldScale, worldScale, 1f);

        float alpha = Mathf.Lerp(0.5f, 0f, timer / fadeDuration);
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (timer > fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
