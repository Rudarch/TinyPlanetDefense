using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationStyle { Passive, Timed, Trigger }
public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxLevel = 1;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private List<UpgradePrerequisite> prerequisites = new();

    [Header("Cooldown Settings")]
    public float cooldownDuration = 0f;
    private float cooldownRemaining = 0f;

    public float CooldownRemaining => cooldownRemaining;
    public float CooldownDuration => cooldownDuration;

    [Header("Activation")]
    public ActivationStyle activationStyle = ActivationStyle.Passive;
    public float activationDuration = 0f;
    private float activationTimer = 0f;

    public bool IsActivated { get; private set; }
    public bool IsReadyForActivation => !IsActivated && cooldownRemaining <= 0f;
    public bool IsMaxedOut => CurrentLevel >= maxLevel;

    public Action OnUpgradeChanged;
    public Action<float, float> OnActivationTimerChanged;

    public int NextLevel => CurrentLevel + 1;

    public int CurrentLevel { get => currentLevel; }

    public void InitializeUpgrade()
    {
        IsActivated = false;
        OnUpgradeChanged = null;
        currentLevel = 0;
        activationTimer = 0;
        ResetInternal();

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

        ApplyUpgradeInternal();

        Debug.Log($"Applied upgrade: (Level {CurrentLevel}/{maxLevel})");
    }

    public virtual bool ArePrerequisitesMet()
    {
        foreach (var prereq in prerequisites)
        {
            if (prereq.requiredUpgrade == null || prereq.requiredUpgrade.CurrentLevel < prereq.minimumLevel)
                return false;
        }
        return true;
    }

    public virtual string GetUpgradeEffectText() => string.Empty;

    public virtual void TickUpgrade(float deltaTime)
    {
        if (cooldownRemaining > 0f)
        {
            cooldownRemaining -= deltaTime;
            if (cooldownRemaining < 0f)
            {
                OnUpgradeChanged?.Invoke();
                cooldownRemaining = 0f;
            }
            OnActivationTimerChanged?.Invoke(cooldownDuration, cooldownRemaining);
        }

        if (activationStyle == ActivationStyle.Timed && IsActivated)
        {
            activationTimer -= deltaTime;
            OnActivationTimerChanged?.Invoke(activationDuration, activationTimer);
            if (activationTimer <= 0f)
            {
                Deactivate();
            }
        }
    }

    public virtual void Activate()
    {
        if (!IsReadyForActivation) return;


        if (activationStyle == ActivationStyle.Timed)
        {
            activationTimer = activationDuration;
        }

        IsActivated = true;
        OnUpgradeChanged?.Invoke();
        ActivateInternal();
        cooldownRemaining = cooldownDuration;
    }

    public virtual void Deactivate()
    {
        IsActivated = false;
        OnUpgradeChanged?.Invoke();
        DeactivateInternal();
    }


    protected virtual void ResetInternal() { }
    protected virtual void ActivateInternal() { }
    protected virtual void InitializeInternal() { }
    protected virtual void DeactivateInternal() { }
    protected virtual void ApplyUpgradeInternal() { }
}
