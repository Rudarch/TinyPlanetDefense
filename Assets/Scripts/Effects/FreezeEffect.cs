using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    public float fadeDuration = 0.4f;
    public Color effectColor = Color.cyan;
    public float maxRadius = 5f;

    private SpriteRenderer sr;
    private float timer = 0f;
    private float expandSpeed;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        if (sr != null)
            sr.color = effectColor;

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
            sr.color = new Color(effectColor.r, effectColor.g, effectColor.b, alpha);

        if (timer > fadeDuration)
            Destroy(gameObject);
    }
}
