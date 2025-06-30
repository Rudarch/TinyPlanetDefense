using UnityEngine;

public enum EnemyRole
{
    Swarm,      // Low cost, high numbers
    Tank,       // Soakers, frontline
    Ranged,     // Weak at close range
    Support,    // Buff/heal/etc.
    Elite       // Expensive, impactful
}

[System.Serializable]
public class EnemyMetadata : MonoBehaviour
{
    public float cost = 1f;
    public EnemyRole role;
    public EnemyRole[] synergyWith;
    public int groupMinCount = 1; // for swarm effectiveness
    public bool requiresMix = false; // e.g. support, ranged
}