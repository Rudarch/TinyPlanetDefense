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

        if (Upgrades.Inst.TwinBarrel.IsActivated)
            EnableTwinMuzzles();
        else
            EnableSingleMuzzle();
    }

    public void EnableTwinMuzzles()
    {
        if (twinMuzzlesGroup != null) twinMuzzlesGroup.SetActive(true);
        if (singleMuzzleGroup != null) singleMuzzleGroup.SetActive(false);
    }

    public void EnableSingleMuzzle()
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

        float cooldownMult = Upgrades.Inst.ReduceCooldown.CooldownReductionMultiplier;
        if (Upgrades.Inst.TwinBarrel.IsActivated)
            cooldownMult *= Upgrades.Inst.TwinBarrel.FireSpeedMultiplier;
        float finalCooldown = baseCooldown * cooldownMult;

        if (Upgrades.Inst.TwinBarrel.IsActivated)
        {
            FireWithChance(muzzleLeft, direction);
            yield return GetWait(twinBarrelDelay * cooldownMult);
            FireWithChance(muzzleRight, direction);
        }
        else
        {
            FireWithChance(muzzleCenter, direction);
        }

        yield return GetWait(finalCooldown);
        isFiring = false;
    }

    void FireWithChance(Transform muzzle, Vector2 direction)
    {
        FireFromMuzzle(muzzle, direction);
        int extraShots = Upgrades.Inst.ExtraShot.GetExtraShotCount();
        for (int i = 0; i < extraShots; i++)
        {
            float delay = Upgrades.Inst.ExtraShot.extraShotInterval * Upgrades.Inst.ReduceCooldown.CooldownReductionMultiplier;
            StartCoroutine(DelayedExtraShot(muzzle, direction, delay));
        }
    }

    IEnumerator DelayedExtraShot(Transform muzzle, Vector2 direction, float delay)
    {
        yield return GetWait(delay);
        FireFromMuzzle(muzzle, direction);
    }

    void FireFromMuzzle(Transform muzzle, Vector2 dir)
    {
        if (muzzle == null) return;

        Vector2 finalDirection = dir;
        if (Upgrades.Inst.TwinBarrel.IsActivated)
        {
            float spreadAngle = Random.Range(-22.5f, 22.5f); // 45° total cone
            finalDirection = Quaternion.Euler(0, 0, spreadAngle) * dir;
        }

        GameObject proj = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        var projectile = proj.GetComponent<Projectile>();

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();

        if (projectile != null)
        {
            projectile.SetDirection(finalDirection);

            if (Upgrades.Inst.OverchargedShot.IsActivated && Time.time >= nextOverchargeTime)
            {
                projectile.ApplyOvercharge(Upgrades.Inst.OverchargedShot.scaleMultiplier);
                nextOverchargeTime = Time.time + Upgrades.Inst.OverchargedShot.Interval;
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
