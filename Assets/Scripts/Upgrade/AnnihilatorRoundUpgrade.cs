using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AnnihilatorRoundUpgrade", menuName = "Upgrades/AnnihilatorRound")]
public class AnnihilatorRoundUpgrade : Upgrade
{
    public GameObject annihilatorPrefab;

    [Header("Firing Settings")]
    public float damage = 100f;
    public float spawnRadius = 0.2f;
    public float spawnDelay = 0.1f;

    private Transform planet;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.AnihilatorRound = this;
        planet = GameObject.FindWithTag("Planet")?.transform;
    }

    protected override void ActivateInternal()
    {
        int shotCount = CurrentLevel;
        if (planet.TryGetComponent(out MonoBehaviour mono))
        {
            mono.StartCoroutine(FireMultipleRounds(shotCount));
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

        this.Deactivate();
    }

    public override string GetUpgradeEffectText()
    {
        return $"Fires {NextLevel} anihilation rounds.";
    }
}
