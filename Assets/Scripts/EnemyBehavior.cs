using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyType enemyType;
    public Transform planetCenter;
    public float movementSpeed = 1f;

    [Header("ZigZag Settings")]
    public float zigzagAmplitude = 0.5f;
    public float zigzagFrequency = 2f;

    private float zigzagOffset;

    void Start()
    {
        zigzagOffset = Random.Range(0f, 2f * Mathf.PI); // Desync zigzag
    }

    void Update()
    {
        if (planetCenter == null || enemyType == EnemyType.Ranged) return;

        Vector2 direction = (planetCenter.position - transform.position).normalized;

        if (enemyType == EnemyType.ZigZag)
        {
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            float offset = Mathf.Sin(Time.time * zigzagFrequency + zigzagOffset) * zigzagAmplitude;
            direction += perpendicular * offset;
            direction.Normalize();
        }

        transform.position += (Vector3)(direction * movementSpeed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}