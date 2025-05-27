using UnityEngine;
using UnityEngine.EventSystems;

public class RotationWheel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public AutoCannonController cannonController;
    public float maxSpinSpeed = 360f; // degrees per second = full boost

    private float previousAngle;
    private float currentBoost = 0f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousAngle = GetAngle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float newAngle = GetAngle(eventData.position);
        float deltaAngle = Mathf.DeltaAngle(previousAngle, newAngle);
        float angularSpeed = deltaAngle / Time.deltaTime;

        float boost = Mathf.Clamp01(Mathf.Abs(angularSpeed) / maxSpinSpeed);
        currentBoost = boost;

        if (cannonController != null)
        {
            cannonController.SetRotationBoost(boost);
        }

        previousAngle = newAngle;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentBoost = 0f;
        if (cannonController != null)
        {
            cannonController.SetRotationBoost(0f);
        }
    }

    float GetAngle(Vector2 screenPos)
    {
        Vector2 dir = screenPos - RectTransformUtility.WorldToScreenPoint(null, transform.position);
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}