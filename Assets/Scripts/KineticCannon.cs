using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticCannon : WeaponSystem
{
    public GameObject projectilePrefab;
    public float baseCooldown = 2f;
    public float twinBarrelDelay = 0.05f;

    [Header("Muzzles")]
    public Transform muzzleCenter;
    public Transform muzzleLeft;
    public Transform muzzleRight;
    public GameObject twinMuzzlesGroup;
    public GameObject singleMuzzleGroup;

    private float nextOverchargeTime = 0f;
    private bool isFiring = false;
    private AudioSource audioSource;

    private static readonly Dictionary<float, WaitForSeconds> waitCache = new();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Upgrades.Inst.twinBarrel.enabled)
            EnableTwinMuzzles();
        else
            EnableSingleMuzzle();
    }

    public void EnableTwinMuzzles()
    {
        if (twinMuzzlesGroup != null) twinMuzzlesGroup.SetActive(true);
        if (singleMuzzleGroup != null) singleMuzzleGroup.SetActive(false);
    }

    private void EnableSingleMuzzle()
    {
        if (twinMuzzlesGroup != null) twinMuzzlesGroup.SetActive(false);
        if (singleMuzzleGroup != null) singleMuzzleGroup.SetActive(true);
    }

    public override void TryFireAt(Transform target)
    {
        if (!isFiring)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            StartCoroutine(FireBurst(dir));
        }
    }

    public override void TryFireWithDirection(Vector2 direction)
    {
        if (!isFiring)
            StartCoroutine(FireBurst(direction));
    }

    IEnumerator FireBurst(Vector2 direction)
    {
        isFiring = true;

        int salvoCount = 1 + Upgrades.Inst.extraShot.shotsPerSalvo;
        float shotInterval = Upgrades.Inst.reduceCooldown.GetShotInterval();
        float finalCooldown = baseCooldown * Upgrades.Inst.reduceCooldown.GetCooldownReductionMultiplier();

        for (int i = 0; i < salvoCount; i++)
        {
            if (Upgrades.Inst.twinBarrel.enabled)
            {
                FireFromMuzzle(muzzleLeft, direction);

                if (twinBarrelDelay > 0f)
                    yield return GetWait(twinBarrelDelay);

                FireFromMuzzle(muzzleRight, direction);
            }
            else
            {
                FireFromMuzzle(muzzleCenter, direction);
            }

            if (i < salvoCount - 1)
                yield return GetWait(shotInterval);
        }

        yield return GetWait(finalCooldown);
        isFiring = false;
    }

    void FireFromMuzzle(Transform muzzle, Vector2 dir)
    {
        if (muzzle == null) return;

        GameObject proj = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        var projectile = proj.GetComponent<Projectile>();

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();

        if (projectile != null)
        {
            projectile.SetDirection(dir);

            var upgrades = Upgrades.Inst;
            if (upgrades.overchargedShot.enabled && Time.time >= nextOverchargeTime)
            {
                projectile.ApplyOvercharge(
                    upgrades.overchargedShot.damageMultiplier,
                    upgrades.overchargedShot.scaleMultiplier
                );
                nextOverchargeTime = Time.time + upgrades.overchargedShot.interval;
            }
        }
    }

    private static WaitForSeconds GetWait(float seconds)
    {
        if (!waitCache.TryGetValue(seconds, out var wait))
        {
            wait = new WaitForSeconds(seconds);
            waitCache[seconds] = wait;
        }
        return wait;
    }
}
