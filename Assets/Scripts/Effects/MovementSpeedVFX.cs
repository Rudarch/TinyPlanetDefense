using UnityEngine;

public class MovementSpeedVFX : MonoBehaviour
{
    public float minAlpha = 0f;
    public float maxAlpha = 0.9f;
    public float shakeStartSpeedPercent = 0.5f;
    public float maxShakeIntensity = 0.2f;
    public float fadeSpeed = 2f;

    SpriteRenderer sr;
    private Enemy enemy;
    private Vector3 originalLocalPos;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        originalLocalPos = transform.localPosition;
        sr.color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (enemy == null || sr == null) return;

        float speedPercent = Mathf.Clamp01(enemy.moveSpeed / enemy.maxMovementSpeed);

        // Fade in
        Color color = sr.color;
        float targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, speedPercent);
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
        sr.color = color;

        // Shake if speed is high enough
        if (speedPercent >= shakeStartSpeedPercent)
        {
            float shakeStrength = Mathf.Lerp(0f, maxShakeIntensity, (speedPercent - shakeStartSpeedPercent) / (1f - shakeStartSpeedPercent));
            Vector3 shakeOffset = Random.insideUnitCircle * shakeStrength;
            transform.localPosition = originalLocalPos + shakeOffset;
        }
        else
        {
            transform.localPosition = originalLocalPos;
        }
    }
}
