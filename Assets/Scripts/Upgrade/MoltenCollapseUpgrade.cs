using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoltenCollapseUpgrade", menuName = "Upgrades/MoltenCollapse")]
public class MoltenCollapseUpgrade : Upgrade
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionDamage = 3f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.MoltenCollapse = this;
    }

    public void TriggerExplosion(Vector3 position)
    {
        var upgrade = Upgrades.Inst.MoltenCollapse;
        if (!upgrade.IsActivated) return;

        if (upgrade.explosionPrefab != null)
        {
            GameObject fx = GameObject.Instantiate(upgrade.explosionPrefab, position, Quaternion.identity);
            MoltenExplosionFX explosionFX = fx.GetComponent<MoltenExplosionFX>();
            if (explosionFX != null)
                explosionFX.SetRadius(upgrade.explosionRadius);
        }

        // Use coroutine to avoid stack overflow
        Upgrades.Inst.StartCoroutine(DelayedAOEDamage(position, upgrade.explosionRadius, upgrade.explosionDamage));
    }

    private IEnumerator DelayedAOEDamage(Vector3 center, float radius, float damage)
    {
        yield return new WaitForEndOfFrame(); // Avoid stack overflow

        var enemies = EnemyManager.Inst.GetEnemiesInRange(center, radius);
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                // Optional: prevent double-dipping with a tag or flag here
                enemy.UpdateHealth(damage);
            }
        }
    }


    public override string GetUpgradeEffectText()
    {
        return $"Burning enemies explode for {explosionDamage} AOE damage.";
    }
}
