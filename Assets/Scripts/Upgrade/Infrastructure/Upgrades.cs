using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public CryoShellsUpgrade cryoShells;
    public IncreaseDamageUpgrade increaseDamage;
    public EMPRoundsUpgrade empRounds;
    public LifeSiphonUpgrade lifeSiphon;
    public ExplosiveRoundsUpgrade explosiveRounds;
    public ExtraShotUpgrade extraShot;
    public HighCaliberUpgrade highCaliber;
    public IncreaseRotationSpeedUpgrade increaseRotationSpeed;
    public OrbitalWingUpgrade orbitalWing;
    public OverchargedShotUpgrade overchargedShot;
    public PiercingAmmoUpgrade piercingAmmo;
    public ReduceCooldownUpgrade reduceCooldown;
    public RicochetUpgrade ricochet;
    public ThermiteRoundsUpgrade thermiteRounds;
    public TwinBarrelUpgrade twinBarrel;

    public List<Upgrade> allUpgrades;
    
    private HashSet<Upgrade> activeUpgrades = new();


    public static Upgrades Inst { get; private set; }
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(this);
            return;
        }
        Inst = this;
    }

    public bool IsUpgradeActive(Upgrade upgrade) => activeUpgrades.Contains(upgrade);

    public void ToggleUpgrade(Upgrade upgrade, bool isActive)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("ToggleUpgrade called with null upgrade.");
            return;
        }

        if (isActive)
        {
            Debug.Log($"Activating upgrade: {upgrade.upgradeName}");
            upgrade.OnActivate();
        }
        else
        {
            Debug.Log($"Deactivating upgrade: {upgrade.upgradeName}");
            upgrade.OnDeactivate();
        }
    }

    public float GetTotalActiveDrain()
    {
        return activeUpgrades.Sum(u => u.energyCostPerSecond);
    }

    public void ForceAllUpgradesOff()
    {
        activeUpgrades.Clear();
        //UpgradeUI.Inst.ForceAllButtonsOff(); TODO;
    }
}
