using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.Normal;
    public float baseSpeed = 2f;
    public float zigzagAmplitude = 1f;
    public float zigzagFrequency = 2f;
    public Transform planetCenter;

    private Rigidbody2D rb;
    private Vector2 directionToPlanet;
    private float timeAlive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        directionToPlanet = ((Vector2)planetCenter.position - rb.position).normalized;
        timeAlive = 0f;
    }

    void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;

        Vector2 finalDirection = directionToPlanet;

        if (enemyType == EnemyType.ZigZag)
        {
            Vector2 perpendicular = Vector2.Perpendicular(directionToPlanet);
            finalDirection += perpendicular * Mathf.Sin(timeAlive * zigzagFrequency) * zigzagAmplitude;
            finalDirection.Normalize();
        }

        rb.linearVelocity = finalDirection * baseSpeed;
    }
}