using UnityEngine;

public class ThrusterFlicker : MonoBehaviour
{
    [SerializeField] float scaleAmplitude = 0.1f;
    [SerializeField] float scaleFrequency = 5f;

    [SerializeField] float alphaMin = 0.5f;
    [SerializeField] float alphaMax = 1f;
    [SerializeField] float alphaFlickerSpeed = 10f;

    Vector3 initialScale;
    SpriteRenderer spriteRenderer;

    float scaleOffsetSeed;
    float alphaOffsetSeed;
    float alphaMinJitter;
    float alphaMaxJitter;

    void Awake()
    {
        initialScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Give each instance some random variation
        scaleOffsetSeed = Random.Range(0f, 100f);
        alphaOffsetSeed = Random.Range(0f, 100f);
        alphaMinJitter = alphaMin + Random.Range(-0.1f, 0.05f);
        alphaMaxJitter = alphaMax + Random.Range(-0.05f, 0.1f);
    }

    void Update()
    {
        // Scale flicker
        float scaleOffset = Mathf.Sin(Time.time * scaleFrequency + scaleOffsetSeed) * scaleAmplitude;
        transform.localScale = initialScale + Vector3.one * scaleOffset;

        // Alpha flicker
        float alphaNoise = Mathf.PerlinNoise(Time.time * alphaFlickerSpeed + alphaOffsetSeed, 0f);
        float alpha = Mathf.Lerp(alphaMinJitter, alphaMaxJitter, alphaNoise);
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
