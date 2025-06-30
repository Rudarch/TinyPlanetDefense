using UnityEngine;

public abstract class EnemyMovementBase : MonoBehaviour
{
    protected Enemy enemy;

    public virtual void Initialize(Enemy owner)
    {
        enemy = owner;
    }

    public abstract void TickMovement();
}
