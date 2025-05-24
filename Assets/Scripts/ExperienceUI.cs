using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI: MonoBehaviour
{
    public LevelManager levelManager;
    public Image experienceBarFill;

    void Update()
    {
        if (levelManager != null && experienceBarFill != null)
        {
            experienceBarFill.fillAmount = levelManager.GetExperiencePercentage();
        }
    }
}