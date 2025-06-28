using UnityEngine;

public abstract class EnemyAbilityBase : MonoBehaviour
{
    protected Enemy enemy;

    public virtual void Initialize(Enemy enemy) => this.enemy = enemy;
    public virtual void OnSpawned() { }
    public virtual void OnUpdate() { }
    public virtual void OnDeath() { }
    public virtual void OnDamaged(float amount) { }
}
