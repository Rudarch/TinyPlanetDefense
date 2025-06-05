using System.Collections;
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

    private float lastFireTime = -Mathf.Infinity;
    private uint shotsFired = 0;

    void Start()
    {
        if (Upgrades.Inst.Cannon.twinBarrelEnabled)
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
        if (Time.time - lastFireTime < baseCooldown * Upgrades.Inst.Cannon.cooldownReductionMultiplier) 
            return;

        lastFireTime = Time.time;
        Vector2 dir = (target.position - transform.position).normalized;

        StartCoroutine(FireBurst(dir));
    }

    public override void TryFireWithDirection(Vector2 direction)
    {
        if (Time.time - lastFireTime < baseCooldown * Upgrades.Inst.Cannon.cooldownReductionMultiplier) return;
        lastFireTime = Time.time;

        StartCoroutine(FireBurst(direction));
    }

    IEnumerator FireBurst(Vector2 direction)
    {
        for (int i = 0; i <= Upgrades.Inst.Cannon.extraShots; i++)
        {
            if (Upgrades.Inst.Cannon.twinBarrelEnabled)
            {
                FireFromMuzzle(muzzleLeft, direction);

                if (twinBarrelDelay > 0f)
                    yield return new WaitForSeconds(twinBarrelDelay);

                FireFromMuzzle(muzzleRight, direction);
            }
            else
            {
                FireFromMuzzle(muzzleCenter, direction);
            }

            if (i < Upgrades.Inst.Cannon.extraShots)
                yield return new WaitForSeconds(Upgrades.Inst.Cannon.shotInterval);
        }
    }

    void FireFromMuzzle(Transform muzzle, Vector2 dir)
    {
        if (muzzle == null) return;

        shotsFired++;
        GameObject proj = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        var projectile = proj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetDirection(dir);
        }
    }

}
