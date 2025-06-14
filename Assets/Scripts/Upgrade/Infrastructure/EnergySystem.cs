using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float regenPerSecond = 3f;
    public float regenPerLevel = 0.5f;

    public static Action<float, float> OnEnergyChanged;

    public Image energyFillImage;
    public TextMeshProUGUI emergyText;

    void Update()
    {
        float totalDrain = Upgrades.Inst.GetTotalActiveDrain();
        var energyDelta = (regenPerSecond - totalDrain);
        var energyDeltaOverTime = energyDelta * Time.deltaTime;
        currentEnergy += energyDeltaOverTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);

        energyFillImage.fillAmount = currentEnergy / maxEnergy;
        var sign = energyDelta < 0 ? "-" : "+";
        emergyText.text = $"{sign}{energyDelta.ToString("F1")}";

        if (currentEnergy <= 0f)
            Upgrades.Inst.ForceDeactivateAll();
    }

    public bool HasEnough(float amount) => currentEnergy >= amount;

    public void OnLevelUp()
    {
        regenPerSecond += regenPerLevel;
    }
}
