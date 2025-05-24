using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TechPointRange
{
    public EnemyType type;
    public int minPoints;
    public int maxPoints;
}

public class TechPointSettings : MonoBehaviour
{
    public static TechPointSettings Instance { get; private set; }

    public List<TechPointRange> techPointRanges = new List<TechPointRange>();

    private Dictionary<EnemyType, Vector2Int> rangeLookup = new Dictionary<EnemyType, Vector2Int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        foreach (var entry in techPointRanges)
        {
            rangeLookup[entry.type] = new Vector2Int(entry.minPoints, entry.maxPoints);
        }
    }

    public Vector2Int GetTechPointRange(EnemyType type)
    {
        return rangeLookup.TryGetValue(type, out var range) ? range : Vector2Int.zero;
    }
}