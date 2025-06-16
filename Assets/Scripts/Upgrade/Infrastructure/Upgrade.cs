using System;
using UnityEngine;
using UnityEngine.Events;

public enum ActivationStyle { Passive, Toggle, Timed }
public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxLevel = 1;
    public int currentLevel = 0;

    [Header("Energy drain")]
    [SerializeField] protected float baseEnergyDrain = 0f;
    [SerializeField] protected float energyDrainPerLevel = 0f;
    public float energyDrainAmount = 0f;

    [Header("Energy activation")]
    [SerializeField] protected float baseActivationEnergyCost = 0f;
    [SerializeField] protected float activationEnergyCostPerLevel = 0f;
    public float activationEnergyAmount = 0f;

    [Header("Activation")]
    public ActivationStyle activationStyle = ActivationStyle.Passive;
    public float activationDuration = 0f;
    private float activationTimer = 0f;

    public bool IsActivated { get; private set; }
    public bool IsReadyForActivation => EnergySystem.Inst.HasEnough(activationEnergyAmount) && !IsActivated;
    public bool IsMaxedOut => currentLevel >= maxLevel;
    //public bool IsEnabled { get => enabled; protected set => enabled = value; }

    public Action<bool> OnActivationChanged;
    public Action<float, float> OnActivationTimerChanged;

    //protected bool enabled = false;
    protected int NextLevel => currentLevel + 1;
    public virtual void Initialize()
    {
        ResetUpgrade();
        InitializeInternal();
    }

    public void ApplyUpgrade()
    {
        if (IsMaxedOut)
        {
            Debug.LogWarning($"{upgradeName} has reached max level.");
            return;
        }

        currentLevel++;

        energyDrainAmount = baseEnergyDrain + (energyDrainPerLevel * currentLevel);
        activationEnergyAmount = baseActivationEnergyCost + (activationEnergyCostPerLevel * currentLevel);

        ApplyUpgradeInternal();

        Debug.Log($"Applied upgrade: (Level {currentLevel}/{maxLevel})");
    }

    protected void ResetUpgrade()
    {
        IsActivated = false;
        currentLevel = 0;
        energyDrainAmount = 0;
        activationEnergyAmount = 0;
        activationTimer = 0;
        OnActivationChanged = null;
    }

    public virtual string GetUpgradeEffectText()
    {
        return string.Empty;
    }

    public virtual float GetEnergyDrainForNextLevel()
    {
        return baseEnergyDrain + (energyDrainPerLevel * NextLevel);
    }

    public virtual float GetEnergyActivationCostForNextLevel()
    {
        return baseActivationEnergyCost + (activationEnergyCostPerLevel * NextLevel);
    }

    protected virtual void ApplyUpgradeInternal() { }

    protected virtual void InitializeInternal() { }

    public void TickUpgrade(float deltaTime)
    {
        if (activationStyle == ActivationStyle.Timed && IsActivated)
        {
            activationTimer -= deltaTime;
            OnActivationTimerChanged.Invoke(activationDuration, activationTimer);
            if (activationTimer <= 0f)
            {
                Deactivate();
            }
        }
    }

    public virtual void Activate()
    {
        if (activationStyle == ActivationStyle.Timed && IsReadyForActivation)
        {
            EnergySystem.Inst.Consume(activationEnergyAmount);
            activationTimer = activationDuration;
        }

        IsActivated = true;
        OnActivationChanged?.Invoke(IsActivated);
        Debug.Log($"{upgradeName} Activated.");
    }

    public virtual void Deactivate()
    {
        IsActivated = false;
        OnActivationChanged?.Invoke(IsActivated);
        Debug.Log($"{upgradeName} Deactivated.");
    }
}
