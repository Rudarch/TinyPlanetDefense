using UnityEngine;

public class LockCanvasRotation : MonoBehaviour
{
    private Quaternion initialRotation;

    void Awake()
    {
        initialRotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
