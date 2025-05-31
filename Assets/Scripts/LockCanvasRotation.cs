using UnityEngine;

public class LockCanvasRotation : MonoBehaviour
{
    private Quaternion initialRotation;

    void Awake()
    {
        initialRotation = Quaternion.identity; // Always face up
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
