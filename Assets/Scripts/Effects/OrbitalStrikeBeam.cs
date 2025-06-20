using System.Collections;
using UnityEngine;

public class OrbitalStrikeBeam : MonoBehaviour
{
    public float delayBeforeHit = 0.25f;
    public float beamDuration = 0.3f;
    public Color beamColor = Color.red;

    private LineRenderer lr;
    private Enemy target;
    private float damage;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.startColor = beamColor;
            lr.endColor = beamColor;
        }
    }

    public void Strike(Enemy targetEnemy, float dmg)
    {
        target = targetEnemy;
        damage = dmg;
        StartCoroutine(DoStrike());
    }

    private IEnumerator DoStrike()
    {
        yield return new WaitForSeconds(delayBeforeHit);

        if (target != null)
        {
            target.TakeDamage(damage);

            if (lr != null)
            {
                Vector3 from = target.transform.position + Vector3.up * 10f;
                Vector3 to = target.transform.position;
                lr.SetPosition(0, from);
                lr.SetPosition(1, to);
            }
        }

        yield return new WaitForSeconds(beamDuration);
        Destroy(gameObject);
    }
}
