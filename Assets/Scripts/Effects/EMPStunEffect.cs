using UnityEngine;
using System.Collections;

public class EMPStunEffect : MonoBehaviour
{
    private Enemy enemy;
    private EnemyStatusIcons statusIcons;
    private const string STATUS_NAME = "Stunned";
    private Coroutine stunRoutine;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null) 
            Destroy(this);

        statusIcons = GetComponentInChildren<EnemyStatusIcons>();
    }

    public void ApplyStun(float duration)
    {

        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        stunRoutine = StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        enemy?.SetStunned(true);
        if (statusIcons != null)
            statusIcons.SetStatusIcon(STATUS_NAME, true);

        yield return new WaitForSeconds(duration);

        enemy?.SetStunned(false);
        if (statusIcons != null)
            statusIcons.SetStatusIcon(STATUS_NAME, false);

        stunRoutine = null;
    }
}
