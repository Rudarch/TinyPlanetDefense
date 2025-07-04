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
    private float[] bladeRadii;
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
        Debug.Log($"{this.GetType().Name} {nameof(Activate)} method called");

        int count = config.GetTotalBlades();
        float angleStep = 360f / count;
        bladeAngles = new float[count];
        bladeRadii = new float[count];

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            bladeAngles[i] = angle;
            bladeRadii[i] = 0f;

            GameObject blade = Instantiate(
                config.bladePrefab,
                orbitCenter.position,               // Start at the center
                Quaternion.identity,
                bladesRoot
            );

            blade.transform.localScale = Vector3.one;

            blades.Add(blade);
            readyBlades.Add(blade.transform);      // Mark as active immediately
        }

        isActive = true;
    }


    public void Deactivate()
    {
        if (!isActive || blades.Count == 0)
            return;

        isActive = false;

        ClearBlades(); // boom
    }


    void Update()
    {
        if (!isActive || config == null) return;

        float speed = config.GetRotationSpeed();

        for (int i = 0; i < blades.Count; i++)
        {
            if (blades[i] == null || !readyBlades.Contains(blades[i].transform)) continue;

            bladeAngles[i] += config.GetRotationSpeed() * Time.deltaTime;

            float currentRadius = bladeRadii[i];
            float targetRadius = Upgrades.Inst.OrbitalBlades.OrbitRadius;

            if (currentRadius < targetRadius)
            {
                currentRadius += Upgrades.Inst.OrbitalBlades.radiusGrowthSpeed * Time.deltaTime;
                if (currentRadius > targetRadius) currentRadius = targetRadius;
                bladeRadii[i] = currentRadius;
            }

            float rad = bladeAngles[i] * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * currentRadius;
            blades[i].transform.position = orbitCenter.position + offset;
            blades[i].transform.rotation = Quaternion.Euler(0f, 0f, bladeAngles[i] - 90f);
        }
    }

    IEnumerator SpiralMove(Transform blade, float startAngle, float startRadius, float endRadius, float duration)
    {
        float timer = 0f;

        if (blade == null)
            yield break;

        while (timer < duration)
        {
            if (blade == null) yield break;

            float t = timer / duration;
            float angle = startAngle;
            float radius = Mathf.Lerp(startRadius, endRadius, t);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            blade.position = orbitCenter.position + offset;
            blade.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            timer += Time.deltaTime;
            yield return null;
        }

        if (blade == null) yield break;

        // Final positioning
        float finalRad = (startAngle + 360f) * Mathf.Deg2Rad;
        blade.position = orbitCenter.position + new Vector3(Mathf.Cos(finalRad), Mathf.Sin(finalRad)) * endRadius;
        blade.rotation = Quaternion.Euler(0f, 0f, startAngle + 360f - 90f);

        readyBlades.Add(blade);
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
