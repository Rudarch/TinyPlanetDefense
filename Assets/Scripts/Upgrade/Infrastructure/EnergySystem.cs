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
    public TextMeshProUGUI energyText;

    public static EnergySystem Inst { get; private set; }
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
    }

    void Update()
    {
        float totalDrain = Upgrades.Inst.GetTotalActiveDrain();
        var energyDelta = (regenPerSecond - totalDrain);
        float deltaTimeEnergy = energyDelta * Time.deltaTime;

        currentEnergy += deltaTimeEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);

        if (energyFillImage != null)
            energyFillImage.fillAmount = currentEnergy / maxEnergy;

        if (energyText != null)
            energyText.text = $"{(energyDelta >= 0 ? "+" : "")}{energyDelta:F1}";

        Upgrades.Inst.TickTimedUpgrades(Time.deltaTime);

        if (currentEnergy <= 0f)
            Upgrades.Inst.ForceDeactivateAll();
    }

    public bool HasEnough(float amount) => currentEnergy >= amount;

    public bool Consume(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            return true;
        }

        return false;
    }

    public void OnLevelUp()
    {
        regenPerSecond += regenPerLevel;
    }

    public void Restore(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }
}
