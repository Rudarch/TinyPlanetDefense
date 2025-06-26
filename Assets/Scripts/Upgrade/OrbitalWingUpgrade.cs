using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalWingUpgrade", menuName = "Upgrades/OrbitalWing")]
public class OrbitalWingUpgrade : Upgrade
{
    [SerializeField] int interceptorsPerLevel = 1;
    public float launchDelay = 0.5f;
    public float spawnOffset = 0.5f;
    protected override void ApplyUpgradeInternal()
    {
        GameObject planet = GameObject.FindWithTag("Planet");
        if (planet != null)
        {
            var system = planet.GetComponent<OrbitalWingSystem>();
            if (system != null)
            {
                for (int i = 0; i < interceptorsPerLevel; i++)
                {
                    system.AddInterceptor();
                }
            }
            else Debug.Log("OrbitalWingSystem is missing on the Planet.");
        }
        else Debug.Log("Planet not found.");
    }

    public override string GetUpgradeEffectText()
    {
        var sOrNotS = interceptorsPerLevel == 1 ? string.Empty : "'s";
        return $"+{interceptorsPerLevel} drone{sOrNotS}({NextLevel * interceptorsPerLevel}total).";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OrbitalWing = this;
    }
}
