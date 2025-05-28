using UnityEngine;

public class SatelliteOrbit : MonoBehaviour
{
    public Transform center; // Planet
    public float orbitRadius = 4f;
    public float orbitSpeed = 30f;

    private float angle;

    void Update()
    {
        if (center == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * orbitRadius;
        transform.position = center.position + offset;
    }
}
