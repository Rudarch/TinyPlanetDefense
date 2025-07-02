using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    public static UpgradePopup Inst { get; private set; }

    [Header("Panels")]
    public GameObject RegularUpgradeSelectionPanel;
    public GameObject TacticalUpgradeSelectionPanel;

    [Header("Layout Roots")]
    public Transform regularUpgradeGridParent;
    public Transform tacticalUpgradeGridParent;

    [Header("Prefabs & Icons")]
    public GameObject regularUpgradeOptionPrefab;
    public GameObject tacticalOptionPrefab;

    [Header("Colors")]
    public Color cannonMasteryColor = new Color(0.3f, 0.6f, 1f);
    public Color highCaliberColor = new Color(1f, 0.4f, 0.3f);
    public Color energyMatrixColor = new Color(0.4f, 1f, 0.8f);
    public Color tacticalProtocolsColor = new Color(0.8f, 0.6f, 1f);

    public Sprite tacticalIcon;

    public ExperienceSystem xpSystem;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }

    public void ShowTacticalChoices()
    {
        Time.timeScale = 0f;

        RegularUpgradeSelectionPanel.SetActive(false);
        TacticalUpgradeSelectionPanel.SetActive(true);

        foreach (Transform child in tacticalUpgradeGridParent)
            Destroy(child.gameObject);

        SpawnTacticalChoice(Upgrades.Inst.CannonMastery, Upgrades.Inst.CannonMastery.icon, cannonMasteryColor);
        SpawnTacticalChoice(Upgrades.Inst.HighCaliber, Upgrades.Inst.HighCaliber.icon, highCaliberColor);
        SpawnTacticalChoice(Upgrades.Inst.EnergyMatrix, Upgrades.Inst.EnergyMatrix.icon, energyMatrixColor);

        SpawnSpecialAction(
            "TACTICAL PROTOCOLS",
            "Choose 1 of 3 Random Ability Upgrades",
            tacticalIcon,
            tacticalProtocolsColor,
            ShowRandomAbilityOptions
        );
    }

    private void SpawnTacticalChoice(Upgrade upgrade, Sprite icon, Color color)
    {
        var go = Instantiate(tacticalOptionPrefab, tacticalUpgradeGridParent);
        var ui = go.GetComponent<TacticalUpgradeSelectionButton>();
        ui.Setup(upgrade, color);
    }

    private void SpawnSpecialAction(string title, string desc, Sprite icon, Color color, UnityEngine.Events.UnityAction action)
    {
        var go = Instantiate(tacticalOptionPrefab, tacticalUpgradeGridParent);
        var ui = go.GetComponent<TacticalUpgradeSelectionButton>();
        ui.SetupAsSpecialAction(title, desc, icon, color, action);
    }

    public void ShowRandomAbilityOptions()
    {
        RegularUpgradeSelectionPanel.SetActive(true);
        TacticalUpgradeSelectionPanel.SetActive(false);

        foreach (Transform child in regularUpgradeGridParent)
            Destroy(child.gameObject);

        List<Upgrade> pool = Upgrades.Inst.RegualarUpgrades
            .FindAll(u => !u.IsMaxedOut && u.ArePrerequisitesMet());

        if (pool.Count == 0)
        {
            OnUpgradeChosen();
            return;
        }

        List<Upgrade> selected = new();
        while (selected.Count < Mathf.Min(3, pool.Count))
        {
            var candidate = pool[Random.Range(0, pool.Count)];
            if (!selected.Contains(candidate))
                selected.Add(candidate);
        }

        foreach (var upgrade in selected)
        {
            var go = Instantiate(regularUpgradeOptionPrefab, regularUpgradeGridParent);
            var ui = go.GetComponent<RegularUpgradeSelectionButton>();
            ui.Setup(upgrade, this);
        }
    }

    public void OnUpgradeChosen()
    {
        RegularUpgradeSelectionPanel.SetActive(false);
        TacticalUpgradeSelectionPanel.SetActive(false);
        xpSystem?.OnUpgradeSelected();
    }
}
