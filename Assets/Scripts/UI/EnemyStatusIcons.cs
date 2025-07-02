using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyStatusIcons : MonoBehaviour
{
    [System.Serializable]
    public class StatusIconEntry
    {
        public string statusName;
        public GameObject iconObject;
    }

    public List<StatusIconEntry> icons = new List<StatusIconEntry>();

    private Dictionary<string, GameObject> iconMap;

    void Awake()
    {
        iconMap = new Dictionary<string, GameObject>();
        foreach (var entry in icons)
        {
            iconMap[entry.statusName] = entry.iconObject;
            if (entry.iconObject != null)
                entry.iconObject.SetActive(false);
        }
    }

    public void SetStatusIcon(string statusName, bool show)
    {
        if (iconMap.ContainsKey(statusName) && iconMap[statusName] != null)
        {
            iconMap[statusName].SetActive(show);
        }
    }
}
