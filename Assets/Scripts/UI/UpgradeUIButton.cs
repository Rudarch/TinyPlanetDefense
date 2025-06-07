using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIButton : MonoBehaviour
{
    public Button button;
    public Image background;
    public Image icon;

    private Upgrade upgrade;

    public void Initialize(Upgrade upgradeData, Sprite upgradeIcon, System.Action<Upgrade> onClick)
    {
        upgrade = upgradeData;
        icon.sprite = upgradeIcon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke(upgrade));
        }

        SetEnabled(false);
    }

    public void SetEnabled(bool enabled)
    {
        background.color = enabled ? Color.white : Color.gray;
    }

    public Upgrade GetUpgrade()
    {
        return upgrade;
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        this.upgrade = upgrade;
    }
}
