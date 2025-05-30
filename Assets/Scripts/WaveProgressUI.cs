using UnityEngine;
using TMPro;

public class WaveProgressUI : MonoBehaviour
{
    public TMP_Text waveText;
    public TMP_Text enemiesLeftText;
    public TMP_Text nextWaveTimerText;

    private int enemiesRemaining = 0;
    private float timeUntilNextWave = -1f;

    public void SetWave(int waveNumber)
    {
        waveText.text = $"Wave: {waveNumber + 1}";
    }

    public void SetEnemiesRemaining(int count)
    {
        enemiesRemaining = count;
        enemiesLeftText.text = $"Enemies: {count}";
    }

    public void SetNextWaveTimer(float time)
    {
        timeUntilNextWave = time;
    }

    void Update()
    {
        if (timeUntilNextWave >= 0f)
        {
            timeUntilNextWave -= Time.deltaTime; 
            int seconds = Mathf.CeilToInt(timeUntilNextWave);
            nextWaveTimerText.text = $"Next Wave In: {seconds}s";
        }
    }
}

