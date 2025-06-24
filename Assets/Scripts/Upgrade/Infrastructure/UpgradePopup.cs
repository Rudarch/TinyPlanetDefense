using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradePopup : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionParent;
    public GameObject popupRoot;

    public ExperienceSystem xpSystem;

    public void Show()
    {
        popupRoot.SetActive(true);

        foreach (Transform child in optionParent)
            Destroy(child.gameObject);

        List<Upgrade> available = new();
        available = Upgrades.Inst.AllUpgrades
            .Where(upg => !upg.IsMaxedOut && upg.ArePrerequisitesMet())
            .ToList();

        if (available.Count == 0)
        {
            popupRoot.SetActive(false);
            FindFirstObjectByType<ExperienceSystem>()?.OnUpgradeSelected();
            return;
        }

        List<Upgrade> selected = new();
        while (selected.Count < Mathf.Min(3, available.Count))
        {
            Upgrade random = available[Random.Range(0, available.Count)];
            if (!selected.Contains(random))
                selected.Add(random);
        }

        foreach (var upgrade in selected)
        {
            var ui = Instantiate(optionPrefab, optionParent);
            var upgradeOptionUI = ui.GetComponent<UpgradeOptionUI>();
            upgradeOptionUI.Setup(upgrade, this);
        }
    }

    public void Hide()
    {
        popupRoot.SetActive(false);

        if (xpSystem != null)
        {
            xpSystem.OnUpgradeSelected();
        }
    }
}