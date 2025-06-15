using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;
    public GameObject ricochetLinePrefab;
    public GameObject explosionEffectPrefab;
    public GameObject impactFlashPrefab;
    public AudioClip hitSound;

    private Vector2 direction;
    private ProjectileHitHandler hitHandler;

    void Start()
    {
        var upgradeState = Upgrades.Inst;
        damage += upgradeState.increaseDamage.BonusDamage;
        damage *= upgradeState.overheatProtocol.OverheatDamageMultiplier;

        hitHandler = gameObject.AddComponent<ProjectileHitHandler>();
        hitHandler.Setup(
            this,
            upgradeState,
            damage,
            upgradeState.piercingAmmo.IsEnabled ? upgradeState.piercingAmmo.pierceCount : 0,
            ricochetLinePrefab,
            explosionEffectPrefab,
            impactFlashPrefab,
            hitSound);

        hitHandler.CheckImmediateOverlap();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void LateUpdate()
    {
        hitHandler.ResetFrame();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void ApplyOvercharge(float damageMultiplier, float scaleMultiplier)
    {
        damage *= damageMultiplier;
        transform.localScale *= scaleMultiplier;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        hitHandler.OnHit(other);

    }
}
