using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float lifetime = 1.2f;
    public float height = 1.5f;
    public float sidewaysStrength = 0.8f;
    public float gravity = -4f;

    private Vector3 velocity;
    private float timer = 0f;
    private TextMeshPro text;
    private Color originalColor;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        originalColor = text.color;

        float angle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
        velocity = new Vector3(Mathf.Sin(angle) * sidewaysStrength, height, 0f);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        timer += dt;

        transform.position += velocity * dt;
        velocity.y += gravity * dt;

        float t = timer / lifetime;
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f - t);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float amount, Color color)
    {
        if (text == null) text = GetComponent<TextMeshPro>();
        text.text = Mathf.RoundToInt(amount).ToString();
        text.color = color;
        originalColor = color;
    }
}