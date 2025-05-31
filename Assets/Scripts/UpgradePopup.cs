using System.Collections.Generic;
using UnityEngine;

public class UpgradePopup : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionParent;
    public GameObject popupRoot;
    public List<CannonUpgrade> allUpgrades;

    private GameObject cannon;
    public void Show(GameObject cannon)
    {
        this.cannon = cannon;
        popupRoot.SetActive(true);

        // Clear old UI
        foreach (Transform child in optionParent)
            Destroy(child.gameObject);

        // Filter available upgrades
        List<CannonUpgrade> available = new();
        foreach (var upgrade in allUpgrades)
        {
            if (!upgrade.isUnique || !UpgradeManager.Instance.IsUniqueUpgradeTaken(upgrade))
                available.Add(upgrade);
        }

        // If no upgrades left, skip level up
        if (available.Count == 0)
        {
            popupRoot.SetActive(false);
            FindFirstObjectByType<CannonXPSystem>()?.OnUpgradeSelected(); // Safely skip
            return;
        }

        // Randomly pick up to 3
        List<CannonUpgrade> selected = new();
        while (selected.Count < Mathf.Min(3, available.Count))
        {
            CannonUpgrade random = available[Random.Range(0, available.Count)];
            if (!selected.Contains(random))
                selected.Add(random);
        }

        // Show UI
        foreach (var upgrade in selected)
        {
            var ui = Instantiate(optionPrefab, optionParent);
            ui.GetComponent<UpgradeOptionUI>().Setup(upgrade, cannon, this);
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