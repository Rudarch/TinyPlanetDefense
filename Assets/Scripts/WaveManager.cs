using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Type Values")]
    public int normalEnemyValue = 1;
    public int fastEnemyValue = 2;
    public int tankEnemyValue = 3;
    public int zigzagEnemyValue = 2;

    public EnemySpawner spawner;
    public List<WaveConfig> waveConfigs = new List<WaveConfig>();
    private int currentWaveIndex = 0;
    private float timer = 0f;
    private bool waveInProgress = false;

    public TextMeshProUGUI  waveText;
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
            enemyTypes = new List<EnemyType>(original.enemyTypes)
        };

        spawner.SpawnConfiguredWave(scaled, GetValueMap());
        currentWaveIndex++;
        waveInProgress = true;
    }

    Dictionary<EnemyType, int> GetValueMap()
    {
        return new Dictionary<EnemyType, int>
        {
            { EnemyType.Normal, normalEnemyValue },
            { EnemyType.Fast, fastEnemyValue },
            { EnemyType.Tank, tankEnemyValue },
            { EnemyType.ZigZag, zigzagEnemyValue }
        };
    }

    void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + ((currentWaveLoop - 1) * waveConfigs.Count + currentWaveIndex).ToString();
        }

        if (enemiesAliveText != null)
        {
            enemiesAliveText.text = "Enemies: " + CountAliveEnemies().ToString();
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