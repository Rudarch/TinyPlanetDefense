using System.Collections;
using UnityEngine;

public class KineticCannon : WeaponSystem
{
    public GameObject projectilePrefab;
    public float baseDamage = 10f;
    public float bonusDamage = 0f;
    public float cooldown = 2f;

    [Header("Pierce Shots")]
    public int extraPierce = 0;

    [Header("Explosive Shots")]
    public bool explosiveEnabled = false;
    public float explosionRadius = 0f;
    public float splashDamageMultiplier = 0.3f;

    [Header("Extra Shots")]
    public int extraShots = 0;
    public float shotInterval = 0.15f;

    [Header("High Caliber")]
    public bool knockbackEnabled = false;
    public float knockbackForce = 5f;
    public float projectileScale = 1f;

    [Header("Ricochet")]
    public bool ricochetEnabled = false;
    public int ricochetCount = 1;
    public float ricochetRange = 5f;

    [Header("Cryo Shells")]
    public bool cryoEnabled = false;
    public float cryoSlowAmount = 0.3f;
    public float cryoSlowDuration = 2f;

    [Header("Twin Barrel")]
    public bool twinBarrelEnabled = false;
    public float twinBarrelDelay = 0.05f; 

    [Header("Muzzles")]
    public Transform muzzleCenter;
    public Transform muzzleLeft;
    public Transform muzzleRight;
    public GameObject twinMuzzlesGroup;
    public GameObject singleMuzzleGroup;

    [Header("Thermite")]
    public bool thermiteEnabled = false;
    public float thermiteDuration = 3f;
    public float thermiteDPS = 1f;

    [Header("Aiming")]
    public float allowedDeviationAngle = 5f;
    public Transform rotatingPart;

    //private bool isFiring = false;
    private float lastFireTime = -Mathf.Infinity;

    void Start()
    {
        if (twinBarrelEnabled)
        {
            EnableTwinMuzzles();
        }
        else
        {
            EnableSingleMuzzle();
        }
    }

    public void EnableTwinMuzzles()
    {
        if (twinMuzzlesGroup != null) 
            twinMuzzlesGroup.SetActive(true);
        if (singleMuzzleGroup != null) 
            singleMuzzleGroup.SetActive(false);
    }

    private void EnableSingleMuzzle()
    {
        if (twinMuzzlesGroup != null) 
            twinMuzzlesGroup.SetActive(false);
        if (singleMuzzleGroup != null) 
            singleMuzzleGroup.SetActive(true);
    }

    public override void TryFireAt(Transform target)
    {
        if (Time.time - lastFireTime < cooldown) 
            return;
        if (!IsLookingAtTarget(target)) 
            return;

        lastFireTime = Time.time;
        Vector2 dir = (target.position - transform.position).normalized;

        StartCoroutine(FireBurst(dir));
    }

    public bool IsLookingAtTarget(Transform target)
    {
        if (target == null || rotatingPart == null)
            return false;

        Vector2 toTarget = (target.position - rotatingPart.position).normalized;
        float angleToTarget = Vector2.SignedAngle(rotatingPart.up, toTarget);

        return Mathf.Abs(angleToTarget) <= allowedDeviationAngle;
    }

    IEnumerator FireBurst(Vector2 direction)
    {
        for (int i = 0; i <= extraShots; i++)
        {
            if (twinBarrelEnabled)
            {
                // Fire from left barrel first
                FireFromMuzzle(muzzleLeft, direction);

                // Small delay before firing from right barrel
                if (twinBarrelDelay > 0f)
                    yield return new WaitForSeconds(twinBarrelDelay);

                FireFromMuzzle(muzzleRight, direction);
            }
            else
            {
                FireFromMuzzle(muzzleCenter, direction);
            }

            if (i < extraShots)
                yield return new WaitForSeconds(shotInterval);
        }
    }

    void FireFromMuzzle(Transform muzzle, Vector2 dir)
    {
        if (muzzle == null) return;

        GameObject proj = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        proj.transform.localScale *= projectileScale;
        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = baseDamage + bonusDamage;

            projectile.pierceCount = extraPierce;

            projectile.isExplosive = explosiveEnabled;
            projectile.explosionRadius = explosionRadius;
            projectile.splashDamageMultiplier = splashDamageMultiplier;

            projectile.knockbackEnabled = knockbackEnabled;
            projectile.knockbackForce = knockbackForce;

            projectile.enableRicochet = ricochetEnabled;
            projectile.ricochetRange = ricochetRange;
            projectile.maxRicochets = ricochetCount;

            projectile.applyCryo = cryoEnabled;
            projectile.cryoSlowAmount = cryoSlowAmount;
            projectile.cryoSlowDuration = cryoSlowDuration;

            projectile.thermiteEnabled = thermiteEnabled;
            projectile.thermiteDuration = thermiteDuration;
            projectile.thermiteDPS = thermiteDPS;

            projectile.SetDirection(dir);
        }
    }

}
