using UnityEngine;

public class DrainEnergyNearPlanet : EnemyAbilityBase
{
    public float range = 3f;
    public float energyDrainPerSecond = 5f;

    public override void OnUpdate()
    {
        if (enemy.planetTarget && Vector2.Distance(enemy.transform.position, enemy.planetTarget.position) < range)
        {
            EnergySystem.Inst.Consume(energyDrainPerSecond * Time.deltaTime);
        }
    }
}
