using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitalBladeController : MonoBehaviour
{
    public Transform orbitCenter;         // Usually the Planet
    public Transform bladesRoot;          // Parent transform for rotating blades
    private List<GameObject> blades = new();
    private List<Coroutine> bladeCoroutines = new();
    private OrbitalBladesUpgrade config;
    private float[] bladeAngles;
    private bool isActive = false;
    private HashSet<Transform> readyBlades = new();

    [Header("Orbit Settings")]
    public float spawnDuration = 0.5f;
    public float despawnDuration = 0.5f;
    public void Setup(OrbitalBladesUpgrade upgrade)
    {
        config = upgrade;
        orbitCenter = transform; // The controller is expected to be attached to the Planet
    }

    public void Activate()
    {
        ClearBlades();

        int count = config.GetTotalBlades();
        float angleStep = 360f / count;
        bladeAngles = new float[count];

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            bladeAngles[i] = angle;

            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 0.2f;
            GameObject blade = Instantiate(config.bladePrefab, orbitCenter.position + offset, Quaternion.identity, bladesRoot);
            blade.transform.localScale = Vector3.zero;

            readyBlades.Clear(); // clear before reuse
            Coroutine routine = StartCoroutine(SpiralMove(blade.transform, angle, 0f, Upgrades.Inst.OrbitalBlades.OrbitRadius, spawnDuration, true));
            bladeCoroutines.Add(routine);

            blades.Add(blade);
        }

        isActive = true;
    }

    public void Deactivate()
    {
        if (!isActive || blades.Count == 0)
            return;

        isActive = false;

        for (int i = 0; i < blades.Count; i++)
        {
            if (blades[i] != null)
            {
                Coroutine routine = StartCoroutine(SpiralMove(blades[i].transform, bladeAngles[i], Upgrades.Inst.OrbitalBlades.OrbitRadius, 0f, despawnDuration, false));
                bladeCoroutines.Add(routine);
            }
        }

        StartCoroutine(CleanupAfterDespawn(despawnDuration));
    }

    void Update()
    {
        if (!isActive || config == null) return;

        float speed = config.GetRotationSpeed();

        for (int i = 0; i < blades.Count; i++)
        {
            if (blades[i] == null || !readyBlades.Contains(blades[i].transform)) continue;

            bladeAngles[i] += speed * Time.deltaTime;
            float rad = bladeAngles[i] * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * Upgrades.Inst.OrbitalBlades.OrbitRadius;

            blades[i].transform.position = orbitCenter.position + offset;
            blades[i].transform.rotation = Quaternion.Euler(0f, 0f, bladeAngles[i] - 90f);
        }
    }

    IEnumerator SpiralMove(Transform blade, float startAngle, float startRadius, float endRadius, float duration, bool scaleUp)
    {
        float timer = 0f;

        if (blade == null)
            yield break;

        Vector3 startScale = scaleUp ? Vector3.zero : blade.localScale;
        Vector3 endScale = scaleUp ? Vector3.one : Vector3.zero;

        while (timer < duration)
        {
            if (blade == null) yield break;

            float t = timer / duration;
            float angle = startAngle + 360f * t;
            float radius = Mathf.Lerp(startRadius, endRadius, t);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            blade.position = orbitCenter.position + offset;
            blade.localScale = Vector3.Lerp(startScale, endScale, t);
            blade.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            timer += Time.deltaTime;
            yield return null;
        }

        if (blade == null) yield break;

        // Final positioning
        float finalRad = (startAngle + 360f) * Mathf.Deg2Rad;
        blade.position = orbitCenter.position + new Vector3(Mathf.Cos(finalRad), Mathf.Sin(finalRad)) * endRadius;
        blade.localScale = endScale;
        blade.rotation = Quaternion.Euler(0f, 0f, startAngle + 360f - 90f);

        if (scaleUp)
        {
            readyBlades.Add(blade);
        }
        else
        {
            Destroy(blade.gameObject);
        }
    }



    IEnumerator CleanupAfterDespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        blades.Clear();
        bladeCoroutines.Clear();
    }

    void ClearBlades()
    {
        foreach (var coroutine in bladeCoroutines)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }

        foreach (var blade in blades)
        {
            if (blade != null) Destroy(blade);
        }

        readyBlades.Clear();
        blades.Clear();
        bladeCoroutines.Clear();
    }
}
