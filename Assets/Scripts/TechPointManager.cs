using UnityEngine;

public class TechPointManager : MonoBehaviour
{
    public static TechPointManager Instance { get; private set; }
    public int totalTechPoints = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    public void AddTechPoints(int amount)
    {
        totalTechPoints += amount;
        Debug.Log("TechPoints: " + totalTechPoints);
    }
}