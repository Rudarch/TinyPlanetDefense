using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float regenPerSecond = 5f;

    public static Action<float, float> OnEnergyChanged;

    public Image energyFillImage;
    public TextMeshProUGUI emergyText;

    void Update()
    {
        float totalDrain = Upgrades.Inst.GetTotalActiveDrain();
        currentEnergy += (regenPerSecond - totalDrain) * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);

        energyFillImage.fillAmount = currentEnergy / maxEnergy;
        emergyText.text = Mathf.Round(currentEnergy).ToString();

        if (currentEnergy <= 0f)
            Upgrades.Inst.ForceDeactivateAll();
    }

    public bool HasEnough(float amount) => currentEnergy >= amount;
}
