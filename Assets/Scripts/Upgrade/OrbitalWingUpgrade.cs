using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalWingUpgrade", menuName = "Upgrades/OrbitalWing")]
public class OrbitalWingUpgrade : Upgrade
{
    protected override void ApplyUpgradeInternal()
    {
        GameObject planet = GameObject.FindWithTag("Planet");
        if (planet != null)
        {
            var system = planet.GetComponent<OrbitalWingSystem>();
            if (system != null)
            {
                system.AddInterceptor();
            }
            else Debug.Log("OrbitalWingSystem is missing on the Planet.");
        }
        else Debug.Log("Planet not found.");
    }

    public override string GetUpgradeEffectText()
    {
        return $"+1 drone. {NextLevel} in total.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OrbitalWing = this;
    }
}
