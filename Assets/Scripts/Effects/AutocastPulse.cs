using UnityEngine;

public class AutocastPulse : MonoBehaviour
{
    public float rotateSpeed = 180f;
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
