using UnityEngine;

[System.Serializable]
public class UpgradePrerequisite
{
    public Upgrade requiredUpgrade;
    public int minimumLevel = 1;
}