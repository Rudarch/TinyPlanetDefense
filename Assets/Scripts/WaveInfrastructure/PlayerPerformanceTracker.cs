using UnityEngine;

public class PlayerPerformanceTracker : MonoBehaviour
{
    [Header("Scoring Window")]
    public float trackingDuration = 30f;

    private float recentEnergyUsed = 0f;
    private float recentEnergyWasted = 0f;
    private int recentEnemiesKilled = 0;
    private float recentPlanetDamage = 0f;
    private float currentEnergyLastFrame;

    private float timeSinceStart;

    public float DifficultyScore =>
        recentEnemiesKilled * 1f +
        (recentEnergyUsed - recentEnergyWasted * 0.5f) -
        recentPlanetDamage * 1.5f;

    void Start()
    {
        currentEnergyLastFrame = EnergySystem.Inst.currentEnergy;
        Planet.OnPlanetHealthChanged += OnPlanetHealthChanged;
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime;

        float energyNow = EnergySystem.Inst.currentEnergy;
        float delta = energyNow - currentEnergyLastFrame;
        currentEnergyLastFrame = energyNow;

        if (delta < 0)
            recentEnergyUsed += -delta;
        else
            recentEnergyWasted += delta;

        // Optional: decay stats over time
        float decay = Time.deltaTime / trackingDuration;
        recentEnergyUsed *= (1f - decay);
        recentEnergyWasted *= (1f - decay);
        recentEnemiesKilled = Mathf.Max(0, recentEnemiesKilled - Mathf.RoundToInt(decay * recentEnemiesKilled));
        recentPlanetDamage *= (1f - decay);
    }

    public void NotifyEnemyKilled()
    {
        recentEnemiesKilled++;
    }

    private void OnPlanetHealthChanged(float current, float max)
    {
        float lost = Mathf.Max(0f, lastPlanetHealth - current);
        recentPlanetDamage += lost;
        lastPlanetHealth = current;
    }

    private float lastPlanetHealth = 100f;
}
