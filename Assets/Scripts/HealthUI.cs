using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image healthBarFill;

    void Update()
    {
        if (playerHealth != null && healthBarFill != null)
        {
            healthBarFill.fillAmount = playerHealth.GetHealthPercentage();
        }
    }
}