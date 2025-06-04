using UnityEngine;

public class EMPShockwaveEffect : MonoBehaviour
{
    public float maxRadius = 3f;
    public float duration = 0.5f;

    private Vector3 initialScale;
    private float timer = 0f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;
        float scale = Mathf.Lerp(0f, maxRadius, t);

        transform.localScale = initialScale * scale;
        var color = GetComponent<SpriteRenderer>().color;
        color.a = 1f - t;
        GetComponent<SpriteRenderer>().color = color;

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
