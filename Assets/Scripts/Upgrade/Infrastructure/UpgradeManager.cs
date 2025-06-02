using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private HashSet<Upgrade> uniqueUpgradesTaken = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MarkUniqueUpgradeTaken(Upgrade upgrade)
    {
        if (upgrade.isUnique)
            uniqueUpgradesTaken.Add(upgrade);
    }

    public bool IsUniqueUpgradeTaken(Upgrade upgrade)
    {
        return upgrade.isUnique && uniqueUpgradesTaken.Contains(upgrade);
    }
}
