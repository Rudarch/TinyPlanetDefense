using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIButton : MonoBehaviour
{
    public Button button;
    public Image background;
    public Image glow;
    public Image icon;

    private Upgrade upgrade;

    public void Initialize(Upgrade upgradeData, System.Action<Upgrade> onClick)
    {
        upgrade = upgradeData;
        icon.sprite = upgradeData.icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke(upgrade));
        }

        SetEnabled(false);
    }

    public void SetEnabled(bool enabled)
    {
        glow.enabled = enabled;
    }

    public Upgrade GetUpgrade()
    {
        return upgrade;
    }
}
