using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    //public Transform followTarget;
    //public Vector3 offset = new Vector3(0, 1f, 0f);

    void Update()
    {
        //if (followTarget != null)
        //{
        //    transform.position = followTarget.position + offset;
        //    transform.rotation = Quaternion.identity; // Face camera
        //}
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
