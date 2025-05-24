using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner spawner;
    public List<WaveConfig> waveConfigs = new List<WaveConfig>();
    private int currentWaveIndex = 0;
    private float timer = 0f;
    private bool waveInProgress = false;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesAliveText;
    public TextMeshProUGUI countdownText;

    private int currentWaveLoop = 1;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        UpdateUI();

        if (!waveInProgress)
        {
            timer += Time.deltaTime;
            float delay = waveConfigs[Mathf.Min(currentWaveIndex, waveConfigs.Count - 1)].delayBeforeNextWave;
            if (timer >= delay)
            {
                StartNextWave();
            }
        }
        else
        {
            if (CountAliveEnemies() == 0)
            {
                waveInProgress = false;
                timer = 0f;
            }
        }
    }

    void StartNextWave()
    {
        if (currentWaveIndex >= waveConfigs.Count)
        {
            currentWaveIndex = 0;
            currentWaveLoop++;
        }

        WaveConfig original = waveConfigs[currentWaveIndex];
        WaveConfig scaled = new WaveConfig
        {
            delayBeforeNextWave = original.delayBeforeNextWave,
            totalValue = original.totalValue * currentWaveLoop,
            enemyValues = new List<EnemyTypeValue>()
        };

        foreach (var pair in original.enemyValues)
        {
            scaled.enemyValues.Add(new EnemyTypeValue
            {
                type = pair.type,
                value = pair.value
            });
        }

        spawner.SpawnConfiguredWave(scaled);
        currentWaveIndex++;
        waveInProgress = true;
    }

    void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + ((currentWaveLoop - 1) * waveConfigs.Count + currentWaveIndex).ToString();
        }

        if (enemiesAliveText != null)
        {
            enemiesAliveText.text = $"Enemies: {CountAliveEnemies().ToString()}";
        }

        if (countdownText != null)
        {
            float delay = waveConfigs[Mathf.Min(currentWaveIndex, waveConfigs.Count - 1)].delayBeforeNextWave;
            countdownText.text = waveInProgress ? "" : "Next Wave In: " + Mathf.CeilToInt(delay - timer) + "s";
        }
    }

    int CountAliveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}