using UnityEngine;

[CreateAssetMenu(fileName = "CryoShellsUpgrade", menuName = "Upgrades/CryoShells")]
public class CryoShellsUpgrade : CannonUpgrade
{
    public float slowAmount = 0.3f; // 30%
    public float slowDuration = 2f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.cryoEnabled = true;
            weapon.cryoSlowAmount = slowAmount;
            weapon.cryoSlowDuration = slowDuration;
        }
    }

    public override string GetEffectText()
    {
        return $"Projectiles slow enemies by {slowAmount * 100f}% for {slowDuration} seconds.";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
