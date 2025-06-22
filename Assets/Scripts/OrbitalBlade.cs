
using UnityEngine;

public class OrbitalBlade : MonoBehaviour
{
    public float damage = 10f;
    public float pierceCooldown = 0.2f;
    public float rotationsPerSecond = 0.5f;

    private float lastHitTime = 0f;
    public Transform visual;

    void Update()
    {
        if (visual != null)
        {
            float degreesPerSecond = rotationsPerSecond * 360f;
            visual.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - lastHitTime < pierceCooldown) return;

        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}
