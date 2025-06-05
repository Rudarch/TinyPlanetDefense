using UnityEngine;

public class ProjectileUpgradeState
{
    public float bonusDamage;

    public bool explosiveEnabled;
    public float explosionRadius;
    public float splashDamageMultiplier;

    public bool cryoEnabled;
    public float cryoAmount;
    public float cryoDuration;

    public bool knockbackEnabled;
    public float knockbackForce;
    public float projectileScale;

    public bool ricochetEnabled;
    public int ricochetCount;
    public float ricochetRange;

    public bool piercingEnabled;
    public int pierceCount;

    public bool thermiteEnabled;
    public float thermiteDuration;
    public float thermiteDPS;

    public bool empEnabled;
    public float empRadius;
    public float empStunDuration;
    public int empShotCounter;
    public int shotsPerEMP;
    public int empLevel;

    public bool energySiphonEnabled;
    public float energySiphonFraction;

    public bool overchargedEnabled;
    public float overchargeInterval;
    public float overchargeDamageMultiplier;
    public float overchargeScaleMultiplier;
}
