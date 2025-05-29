using UnityEngine;
using UnityEngine.UI;

public class PlanetHealthUI : MonoBehaviour
{
    public Image fillImage;

    void OnEnable()
    {
        Planet.OnPlanetHealthChanged += UpdateBar;
    }

    void OnDisable()
    {
        Planet.OnPlanetHealthChanged -= UpdateBar;
    }

    void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
