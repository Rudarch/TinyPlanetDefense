
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalBladesUpgrade", menuName = "Upgrades/OrbitalBlades")]
public class OrbitalBladesUpgrade : Upgrade
{
    public GameObject bladePrefab;
    public int baseBladeCount = 2;
    public int bladesPerLevel = 1;
    public float baseRotationSpeed = 60f;
    public float rotationSpeedPerLevel = 15f;
    public float damageInterval = 1f;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float damage = 2f;
    public float OrbitRadius => orbitRadius;
    public float Damage => damage;

    private OrbitalBladeController controller;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OrbitalBlades = this;
    }

    public override void Activate()
    {
        base.Activate();
        if (controller == null)
        {
            var planet = GameObject.FindWithTag("Planet");
            if (planet != null)
                controller = planet.GetComponentInChildren<OrbitalBladeController>();
        }

        if (controller != null)
        {
            controller.Setup(this);
            controller.Activate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        controller?.Deactivate();
    }

    public int GetTotalBlades() => baseBladeCount + CurrentLevel * bladesPerLevel;
    public float GetRotationSpeed() => baseRotationSpeed + CurrentLevel * rotationSpeedPerLevel;

    public override string GetUpgradeEffectText()
    {
        return $"+{bladesPerLevel} blade(s), +{rotationSpeedPerLevel}° speed";
    }
}
