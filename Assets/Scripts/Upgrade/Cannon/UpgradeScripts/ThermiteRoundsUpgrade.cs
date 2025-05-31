using UnityEngine;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : CannonUpgrade
{
    public float burnDuration = 3f;
    public float dps = 1f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.thermiteEnabled = true;
            weapon.thermiteDuration = burnDuration;
            weapon.thermiteDPS = dps;
        }
    }

    public override string GetEffectText()
    {
        return $"Enemies hit are burned for {burnDuration}s ({dps}/s).";
    }
}
