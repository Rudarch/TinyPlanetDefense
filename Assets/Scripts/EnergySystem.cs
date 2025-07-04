using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float baseMaxEnergy = 100f;
    public float currentEnergy = 100f;
    public float baseRegenPerSecond = 3f;
    public float regenPerLevel = 0.5f;
    private float regenMultiplier = 1f;

    public static Action<float, float> OnEnergyChanged;

    public Image energyFillImage;
    public TextMeshProUGUI energyDeltaText;
    public TextMeshProUGUI currentEnergyText;

    public static EnergySystem Inst { get; private set; }
    public float MaxEnergy { get => baseMaxEnergy + Upgrades.Inst.EnergyMatrix.BonusMaxEnergy; }
    public float EnergyRegen { get => (baseRegenPerSecond + Upgrades.Inst.EnergyMatrix.BonusEnergyRegen) * regenMultiplier; }

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
        float totalDrain = 0;
        var energyDelta = (EnergyRegen - totalDrain);
        float deltaTimeEnergy = energyDelta * Time.deltaTime;

        currentEnergy += deltaTimeEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, MaxEnergy);

        OnEnergyChanged?.Invoke(currentEnergy, MaxEnergy);

        if (energyFillImage != null)
            energyFillImage.fillAmount = currentEnergy / MaxEnergy;

        if (energyDeltaText != null)
            energyDeltaText.text = $"{(energyDelta >= 0 ? "+" : "")}{energyDelta:F1}";

        if (currentEnergyText != null)
            currentEnergyText.text = $"{currentEnergy:F0}";
    }

    public bool HasEnough(float amount) => currentEnergy >= amount;

    public bool Consume(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, MaxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy, MaxEnergy);
            return true;
        }

        return false;
    }

    public void SetRegenMultiplier(float multiplier)
    {
        regenMultiplier = multiplier; // Multiply regenPerSecond internally
    }

    public void OnLevelUp()
    {
        baseRegenPerSecond += regenPerLevel;
    }

    public void Restore(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, MaxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, MaxEnergy);
    }
}
