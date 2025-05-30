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
        Time.timeScale = 0f;

        // Clear existing options
        foreach (Transform child in optionParent)
        {
            Destroy(child.gameObject);
        }

        // Pick 3 random upgrades
        List<CannonUpgrade> selected = new List<CannonUpgrade>();
        while (selected.Count < 3 && allUpgrades.Count > 0)
        {
            CannonUpgrade random = allUpgrades[Random.Range(0, allUpgrades.Count)];
            if (!selected.Contains(random))
                selected.Add(random);
        }

        // Spawn UI buttons
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