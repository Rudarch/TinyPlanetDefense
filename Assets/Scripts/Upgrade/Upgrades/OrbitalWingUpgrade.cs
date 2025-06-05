using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalWingUpgrade", menuName = "Upgrades/OrbitalWing")]
public class OrbitalWingUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

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
        return $"Adds 1 drone which automatically attacks enemies.";
    }
}
