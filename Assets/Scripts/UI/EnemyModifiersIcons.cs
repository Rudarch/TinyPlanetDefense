using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyModifierIcons : MonoBehaviour
{
    [System.Serializable]
    public class ModifierIconEntry
    {
        public EnemyModifierType modifierName;
        public GameObject iconObject;
    }

    public List<ModifierIconEntry> icons = new List<ModifierIconEntry>();

    private Dictionary<EnemyModifierType, GameObject> iconMap;

    void Awake()
    {
        iconMap = new Dictionary<EnemyModifierType, GameObject>();
        foreach (var entry in icons)
        {
            iconMap[entry.modifierName] = entry.iconObject;
            if (entry.iconObject != null)
                entry.iconObject.SetActive(false);
        }
    }

    public void SetModifierIcon(EnemyModifierType modifierName, bool show = true)
    {
        if (iconMap.ContainsKey(modifierName) && iconMap[modifierName] != null)
        {
            iconMap[modifierName].SetActive(show);
        }
    }
}
