using UnityEngine;

public abstract class EnemyMovementBase : MonoBehaviour
{
    protected Enemy enemy;
    public bool enableWiggle = false;
    public float wiggleFrequency = 6f;
    public float wiggleAmplitude = 0.1f;

    public virtual void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
    }

    protected Vector3 ApplyWiggleOffset(Vector3 basePosition, Vector3 direction)
    {
        if (!enableWiggle) return basePosition;

        // Get perpendicular vector to motion direction
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
        float offset = Mathf.Sin(Time.time * wiggleFrequency + enemy.GetInstanceID()) * wiggleAmplitude;
        return basePosition + perpendicular * offset;
    }

    public abstract void TickMovement();
}
