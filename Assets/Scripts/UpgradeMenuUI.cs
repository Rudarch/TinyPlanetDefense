using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class UpgradeMenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public Button[] upgradeButtons;

    [Range(0f, 1f)] public float rareChance = 0.2f;
    [Range(0f, 1f)] public float epicChance = 0.05f;

    public Color commonColor = Color.white;
    public Color rareColor = Color.cyan;
    public Color epicColor = Color.magenta;

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        LevelManager.Instance.onLevelUp.AddListener(ShowUpgradeOptions);
    }

    void ShowUpgradeOptions()
    {
        menuPanel.SetActive(true);

        List<UpgradeType> allUpgrades = new List<UpgradeType>((UpgradeType[])System.Enum.GetValues(typeof(UpgradeType)));
        List<UpgradeType> selected = new List<UpgradeType>();

        while (selected.Count < 3)
        {
            UpgradeType random = allUpgrades[UnityEngine.Random.Range(0, allUpgrades.Count)];
            if (!selected.Contains(random))
                selected.Add(random);
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            var upgrade = selected[i];
            var grade = GetRandomGrade();
            float multiplier = GetMultiplier(grade);

            var button = upgradeButtons[i];
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = GetUpgradeName(upgrade, multiplier, grade);
            }

            Color gradeColor = GetGradeColor(grade);
            var colors = button.colors;
            colors.normalColor = gradeColor;
            colors.highlightedColor = gradeColor;
            colors.pressedColor = gradeColor;
            colors.selectedColor = gradeColor;
            button.colors = colors;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                UpgradeManager.Instance.ApplyUpgrade(upgrade, multiplier);
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
            });
        }
    }

    UpgradeGrade GetRandomGrade()
    {
        float roll = Random.value;
        if (roll < epicChance) return UpgradeGrade.Epic;
        if (roll < rareChance + epicChance) return UpgradeGrade.Rare;
        return UpgradeGrade.Common;
    }

    float GetMultiplier(UpgradeGrade grade)
    {
        switch (grade)
        {
            case UpgradeGrade.Rare: return 2f;
            case UpgradeGrade.Epic: return 3f;
            default: return 1f;
        }
    }

    string GetUpgradeName(UpgradeType type, float multiplier, UpgradeGrade grade)
    {
        string prefix = grade.ToString().ToUpper() + ": ";
        switch (type)
        {
            case UpgradeType.FireCooldown: return $"{prefix}{Environment.NewLine}Reduce Fire Cooldown {Environment.NewLine}(-" + (10 * multiplier) + "%)";
            case UpgradeType.Damage: return $"{prefix}{Environment.NewLine}Increase Damage {Environment.NewLine}(+" + (20 * multiplier) + "%)";
            case UpgradeType.RotationSpeed: return $"{prefix}{Environment.NewLine}Improve Rotation {Environment.NewLine}(+" + (10 * multiplier) + "%)";
            case UpgradeType.FiringRange: return $"{prefix}{Environment.NewLine}Increase Range {Environment.NewLine}(+" + (5 * multiplier) + "%)";
            case UpgradeType.MaxHealth: return $"{prefix}{Environment.NewLine}Increase Max Health {Environment.NewLine}(+" + (20 * multiplier) + "%)";
            default: return type.ToString();
        }
    }

    Color GetGradeColor(UpgradeGrade grade)
    {
        switch (grade)
        {
            case UpgradeGrade.Rare: return rareColor;
            case UpgradeGrade.Epic: return epicColor;
            default: return commonColor;
        }
    }
}