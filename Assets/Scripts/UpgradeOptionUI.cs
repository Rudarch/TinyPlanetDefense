using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Button selectButton;

    private CannonUpgrade upgrade;
    private GameObject cannon;
    private UpgradePopup popup;

    public void Setup(CannonUpgrade upgrade, GameObject cannon, UpgradePopup popup)
    {
        this.upgrade = upgrade;
        this.cannon = cannon;
        this.popup = popup;

        icon.sprite = upgrade.icon;
        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        upgrade.ApplyUpgrade(cannon);
        popup.Hide();
    }
}