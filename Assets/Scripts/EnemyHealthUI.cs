using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Canvas healthCanvas;
    public Image healthFill;
    public EnemyHealth enemyHealth;

    private void Start()
    {
        if (healthCanvas != null)
        {
            healthCanvas.enabled = false;
        }
    }

    private void Update()
    {
        if (enemyHealth != null && healthFill != null && healthCanvas != null)
        {
            float percent = Mathf.Clamp01((float)enemyHealth.health / enemyHealth.maxHealth);
            healthFill.fillAmount = percent;

            // Show canvas only when enemy is not full HP
            healthCanvas.enabled = (percent < 1f && percent > 0f);
        }
    }
}