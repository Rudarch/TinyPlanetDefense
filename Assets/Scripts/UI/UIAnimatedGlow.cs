using UnityEngine;
using UnityEngine.UI;

public class UIAnimatedGlow : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float rotationChangeInterval = 2f;

    public float alphaMin = 0.2f;
    public float alphaMax = 0.8f;
    public float alphaSpeed = 1f;

    public float scaleMin = 0.95f;
    public float scaleMax = 1.05f;
    public float scaleSpeed = 1f;

    private Image image;
    private float rotationDirection = 1f;
    private float rotationTimer = 0f;
    private float alphaT = 0f;
    private float scaleT = 0f;

    void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning("UIAnimatedGlow requires an Image component.");
            enabled = false;
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // Change rotation direction
        rotationTimer += dt;
        if (rotationTimer >= rotationChangeInterval)
        {
            rotationTimer = 0f;
            rotationDirection = Random.value > 0.5f ? 1f : -1f;
        }

        // Apply rotation
        transform.Rotate(0f, 0f, rotationDirection * rotationSpeed * dt);

        // Fade alpha
        alphaT += alphaSpeed * dt;
        float alpha = Mathf.Lerp(alphaMin, alphaMax, (Mathf.Sin(alphaT) + 1f) / 2f);
        Color color = image.color;
        color.a = alpha;
        image.color = color;

        // Scale pulse
        scaleT += scaleSpeed * dt;
        float scale = Mathf.Lerp(scaleMin, scaleMax, (Mathf.Sin(scaleT) + 1f) / 2f);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}