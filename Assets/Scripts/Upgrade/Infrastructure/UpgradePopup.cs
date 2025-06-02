using System.Collections.Generic;
using UnityEngine;

public class UpgradePopup : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionParent;
    public GameObject popupRoot;
    public List<Upgrade> allUpgrades;

    private GameObject cannon;
    public void Show(GameObject cannon)
    {
        this.cannon = cannon;
        popupRoot.SetActive(true);

        foreach (Transform child in optionParent)
            Destroy(child.gameObject);

        List<Upgrade> available = new();
        foreach (var upgrade in allUpgrades)
        {
            if (!upgrade.isUnique || !UpgradeManager.Instance.IsUniqueUpgradeTaken(upgrade))
                available.Add(upgrade);
        }

        if (available.Count == 0)
        {
            popupRoot.SetActive(false);
            FindFirstObjectByType<CannonXPSystem>()?.OnUpgradeSelected();
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

        var xpSystem = cannon.GetComponent<CannonXPSystem>();
        if (xpSystem != null)
        {
            xpSystem.OnUpgradeSelected();
        }
    }
}