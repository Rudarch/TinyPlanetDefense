using UnityEngine;
using System.Collections;

public class EnemySlow : MonoBehaviour
{
    private Enemy enemy;
    private float originalSpeed;
    private Coroutine slowRoutine;

    private SpriteRenderer sr;
    private Color originalColor;
    private EnemyStatusIcons statusIcons;
    private const string STATUS_NAME = "Slowed";
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        sr = GetComponentInChildren<SpriteRenderer>();
        statusIcons = GetComponentInChildren<EnemyStatusIcons>();
        originalSpeed = enemy.moveSpeed;
    }

    public void ApplySlow(float amount, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowRoutine(amount, duration));
    }

    IEnumerator SlowRoutine(float amount, float duration)
    {
        enemy.moveSpeed = originalSpeed * (1f - amount);
        if (sr != null) 
            sr.color = Color.cyan;
        if (statusIcons != null) 
            statusIcons.SetStatusIcon(STATUS_NAME, true);

        yield return new WaitForSeconds(duration);

        enemy.moveSpeed = originalSpeed;
        if (sr != null) 
            sr.color = originalColor;
        if (statusIcons != null) 
            statusIcons.SetStatusIcon(STATUS_NAME, false);

        slowRoutine = null;
    }
}
