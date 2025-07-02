using UnityEngine;

public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Scoring Window")]
    public float trackingDuration = 30f;

    private float currentPlanetHealth = 100f;
    private float maxPlanetHealth = 100f;

    private int lastPlanetDamageWave = -10;
    private int currentWave = 0;

    [SerializeField] private float missingHealthPenalty = 50f;
    [SerializeField] private int missingHealthDecayWaves = 6;
    private float recentEnergyUsed = 0f;
    private float recentEnergyWasted = 0f;
    private float recentEnemyKillValue = 0f;
    private float recentPlanetDamage = 0f;
    private float currentEnergyLastFrame;

    private float lastPlanetHealth = 100f;

    public float DifficultyScore
    {
        get
        {
            float score = recentEnemyKillValue +
                          (recentEnergyUsed - recentEnergyWasted * 0.5f) -
                          recentPlanetDamage * 3f;

            float missing = maxPlanetHealth - currentPlanetHealth;
            bool recentlyDamaged = (currentWave - lastPlanetDamageWave) <= missingHealthDecayWaves;

            if (recentlyDamaged && missing > 0f)
                score -= missingHealthPenalty * (missing / maxPlanetHealth);

            return score;
        }
    }
    public void SetCurrentWave(int waveIndex)
    {
        currentWave = waveIndex;
    }

    void Start()
    {
        currentEnergyLastFrame = EnergySystem.Inst.currentEnergy;
        Planet.OnPlanetHealthChanged += OnPlanetHealthChanged;
    }

    void Update()
    {
        float energyNow = EnergySystem.Inst.currentEnergy;
        float delta = energyNow - currentEnergyLastFrame;
        currentEnergyLastFrame = energyNow;

        if (delta < 0)
            recentEnergyUsed += -delta;
        else
            recentEnergyWasted += delta;

        float decay = Time.deltaTime / trackingDuration;
        recentEnergyUsed *= (1f - decay);
        recentEnergyWasted *= (1f - decay);
        recentEnemyKillValue *= (1f - decay);
        recentPlanetDamage *= (1f - decay);
    }

    public void NotifyEnemyKilled(GameObject enemy)
    {
        var meta = enemy.GetComponent<EnemyMetadata>();
        if (meta != null)
            recentEnemyKillValue += meta.cost;
        else
            recentEnemyKillValue += 1f; // fallback
    }

    private void OnPlanetHealthChanged(float current, float max)
    {
        float lost = Mathf.Max(0f, lastPlanetHealth - current);
        recentPlanetDamage += lost;
        lastPlanetHealth = current;

        currentPlanetHealth = current;
        maxPlanetHealth = max;

        if (lost > 0f)
            lastPlanetDamageWave = currentWave;
    }
}