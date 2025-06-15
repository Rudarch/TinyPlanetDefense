using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    public Image fillImage;

    public void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
