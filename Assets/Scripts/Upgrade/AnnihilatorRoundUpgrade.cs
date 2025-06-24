using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AnnihilatorRoundUpgrade", menuName = "Upgrades/AnnihilatorRound")]
public class AnnihilatorRoundUpgrade : Upgrade
{
    public GameObject annihilatorPrefab;

    [Header("Firing Settings")]
    public float fireInterval = 6f;
    public float damage = 100f;
    public float spawnRadius = 0.2f;
    public float spawnDelay = 0.1f;

    private float timer = 0f;
    private Transform planet;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.AnihilatorRound = this;
        planet = GameObject.FindWithTag("Planet")?.transform;
    }

    public override void Activate()
    {
        if (!IsReadyForActivation) return;

        base.Activate();
        timer = fireInterval;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        timer = 0f;
    }

    public override void TickUpgrade(float deltaTime)
    {
        base.TickUpgrade(deltaTime);

        if (!IsActivated || planet == null) return;

        timer -= deltaTime;
        if (timer <= 0f)
        {
            int shotCount = CurrentLevel; // base 1 + level bonus
            if (planet.TryGetComponent(out MonoBehaviour mono))
            {
                mono.StartCoroutine(FireMultipleRounds(shotCount));
            }
            timer = fireInterval;
        }
    }

    private IEnumerator FireMultipleRounds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = planet.position + new Vector3(offset.x, offset.y, 0f);

            GameObject round = Instantiate(annihilatorPrefab, spawnPos, Quaternion.identity);
            if (round.TryGetComponent(out AnnihilatorRound roundScript))
            {
                roundScript.damage = damage;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"Fires {1 + CurrentLevel} rounds every {fireInterval:F1}s (delay {spawnDelay:F1}s)";
    }
}
