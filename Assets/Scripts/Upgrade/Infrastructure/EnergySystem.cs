using System;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float regenPerSecond = 5f;

    public static Action<float, float> OnEnergyChanged;

    void Update()
    {
        float totalDrain = Upgrades.Inst.GetTotalActiveDrain();
        currentEnergy += (regenPerSecond - totalDrain) * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);

        if (currentEnergy <= 0f)
            Upgrades.Inst.ForceAllUpgradesOff();
    }

    public bool HasEnough(float amount) => currentEnergy >= amount;
}
