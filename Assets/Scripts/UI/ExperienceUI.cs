using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    public Image fillImage;
    public float fillSpeed = 3f; // speed of interpolation

    private float targetFill = 0f;

    void Update()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);
        }
    }

    public void UpdateBar(float current, float max)
    {
        targetFill = current / max;
    }
}
