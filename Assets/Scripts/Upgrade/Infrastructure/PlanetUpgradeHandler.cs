using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetUpgradeHandler : MonoBehaviour
{
    private readonly Dictionary<string, Coroutine> activeEffects = new();

    public void RegisterEffect(string effectName, Func<IEnumerator> effect)
    {
        if (!activeEffects.ContainsKey(effectName))
            activeEffects.Add(
                effectName, 
                StartCoroutine(effect.Invoke()));
    }

    public void UnregisterEffect(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out var coroutine))
        {
            StopCoroutine(coroutine);
            activeEffects.Remove(effectName);
        }
    }
}
