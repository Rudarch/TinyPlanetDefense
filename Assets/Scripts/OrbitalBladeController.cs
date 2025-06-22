
using UnityEngine;
using System.Collections.Generic;
public class OrbitalBladeController : MonoBehaviour
{
    public Transform orbitCenter;         // The planet
    public Transform bladesRoot;          // The rotating group
    private List<GameObject> blades = new();
    private OrbitalBladesUpgrade config;
    private bool isActive = false;
    private float[] bladeAngles;

    public void Setup(OrbitalBladesUpgrade upgrade)
    {
        config = upgrade;
        orbitCenter = transform;
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

            GameObject blade = Instantiate(config.bladePrefab, orbitCenter.position, Quaternion.identity, bladesRoot);
            blades.Add(blade);
        }

        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
        ClearBlades();
    }

    void Update()
    {
        if (!isActive || config == null) return;

        float speed = config.GetRotationSpeed();
        for (int i = 0; i < blades.Count; i++)
        {
            bladeAngles[i] += speed * Time.deltaTime;
            float rad = bladeAngles[i] * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 2f;
            blades[i].transform.position = orbitCenter.position + offset;
            blades[i].transform.rotation = Quaternion.Euler(0f, 0f, bladeAngles[i] - 90f);
        }
    }

    void ClearBlades()
    {
        foreach (var blade in blades)
        {
            if (blade != null) Destroy(blade);
        }
        blades.Clear();
    }
}
