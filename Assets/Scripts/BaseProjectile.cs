using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Core Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    protected Vector2 direction;

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public virtual void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    protected abstract void HandleHit(Collider2D other);

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
    }
}
