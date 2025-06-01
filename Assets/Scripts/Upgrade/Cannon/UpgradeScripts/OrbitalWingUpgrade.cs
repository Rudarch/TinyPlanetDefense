using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalWingUpgrade", menuName = "Upgrades/OrbitalWing")]
public class OrbitalWingUpgrade : CannonUpgrade
{
    public override void ApplyUpgrade(GameObject cannon)
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

    public override string GetEffectText()
    {
        return "Adds an interceptor that patrols and attacks enemies";
    }
}
