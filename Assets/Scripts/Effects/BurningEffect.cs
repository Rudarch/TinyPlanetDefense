using UnityEngine;

public class BurningEffect : MonoBehaviour
{
    public float burnDuration = 3f;
    public float baseDamagePerSecond = 1f;

    private float currentDPS;
    private float timer = 0f;
    private float tickInterval = 0.5f;
    private float tickTimer = 0f;
    private int stackCount = 1;

    private Enemy enemy;
    private EnemyStatusIcons statusIcons;
    private const string STATUS_NAME = "Burning";

    void Start()
    {
        enemy = GetComponent<Enemy>();
        statusIcons = GetComponentInChildren<EnemyStatusIcons>();

        if (statusIcons != null)
        {
            statusIcons.SetStatusIcon(STATUS_NAME, true);
        }

        timer = burnDuration;
        currentDPS = baseDamagePerSecond;
    }

    void Update()
    {
        if (enemy == null || timer <= 0f) return;

        timer -= Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            enemy.TakeDamage(currentDPS * tickInterval);
        }

        if (timer <= 0f)
        {
            if (statusIcons != null)
                statusIcons.SetStatusIcon(STATUS_NAME, false);

            Destroy(this);
        }
    }

    public void ApplyOrRefresh(float baseDPS, float duration)
    {
        timer = duration;
        tickTimer = 0f;

        if (stackCount == 1)
        {
            baseDamagePerSecond = baseDPS;
            currentDPS = baseDPS;
        }

        stackCount++;
        currentDPS = baseDPS * (1f + 0.5f * (stackCount - 1));
    }
}
