using UnityEngine;
using UnityEngine.UI;

public class PlanetHealthUI : MonoBehaviour
{
    public Image fillImage;

    private float targetFill = 0f;
    public float fillSpeed = 3f;

    void Update()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);
        }
    }
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
        targetFill = current / max;
    }
}
