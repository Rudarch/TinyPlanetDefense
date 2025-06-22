using UnityEngine;

public class OrbitalBlade : MonoBehaviour
{
    public float rotationsPerSecond = 1f;

    public Transform visual;

    void Update()
    {
        float spinAngle = 360f * rotationsPerSecond * Time.deltaTime;
        visual.Rotate(0f, 0f, spinAngle);
    }
}
