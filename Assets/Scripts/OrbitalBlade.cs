using UnityEngine;
using System.Collections.Generic;

public class OrbitalBlade : MonoBehaviour
{
    public float rotationsPerSecond = 1f;
    public Transform visual;

    public float damageInterval = 0.2f;

    private Dictionary<Enemy, float> cooldowns = new();
    private List<Enemy> toAddCooldowns = new();

    void Update()
    {
        float spinAngle = 360f * rotationsPerSecond * Time.deltaTime;
        visual.Rotate(0f, 0f, spinAngle);

        // Create a snapshot of keys to avoid modification exception
        List<Enemy> keysSnapshot = new(cooldowns.Keys);
        List<Enemy> toRemove = null;

        foreach (var key in keysSnapshot)
        {
            if (key == null) continue;

            cooldowns[key] -= Time.deltaTime;
            if (cooldowns[key] <= 0f)
            {
                toRemove ??= new();
                toRemove.Add(key);
            }
        }

        if (toRemove != null)
        {
            foreach (var enemy in toRemove)
                cooldowns.Remove(enemy);
        }

        // Add new cooldowns
        if (toAddCooldowns.Count > 0)
        {
            foreach (var enemy in toAddCooldowns)
                if (enemy != null)
                    cooldowns[enemy] = damageInterval;
            toAddCooldowns.Clear();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!Upgrades.Inst.OrbitalBlades.IsActivated) return;

        if (other.TryGetComponent(out Enemy enemy))
        {
            if (!cooldowns.ContainsKey(enemy))
            {
                enemy.TakeDamage(Upgrades.Inst.OrbitalBlades.Damage);
                toAddCooldowns.Add(enemy);
            }
        }
    }
}
