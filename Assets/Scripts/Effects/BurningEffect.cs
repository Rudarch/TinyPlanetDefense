using UnityEngine;

public class BurningEffect : MonoBehaviour
{
    public float burnDuration = 3f;
    public float baseDamagePerSecond = 1f;

    private float currentDPS;
    private float remainingTime = 0f;
    private float tickInterval = 0.5f;
    private float tickTimer = 0f;
    private int stackCount = 1;

    private Enemy enemy;
    private EnemyStatusIcons statusIcons;
    private const string STATUS_NAME = "Burning";

    public bool IsActive()
    {
        return remainingTime > 0f;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
        statusIcons = GetComponentInChildren<EnemyStatusIcons>();

        if (statusIcons != null)
        {
            statusIcons.SetStatusIcon(STATUS_NAME, true);
        }

        remainingTime = burnDuration;
        currentDPS = baseDamagePerSecond;
    }

    void Update()
    {
        if (enemy == null || remainingTime <= 0f) return;

        remainingTime -= Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            enemy.UpdateHealth(currentDPS * tickInterval);
        }

        if (remainingTime <= 0f)
        {
            if (statusIcons != null)
                statusIcons.SetStatusIcon(STATUS_NAME, false);

            Destroy(this);
        }
    }

    public void ApplyOrRefresh(float baseDPS, float duration)
    {
        remainingTime = duration;
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
