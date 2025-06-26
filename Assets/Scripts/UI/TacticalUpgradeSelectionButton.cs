using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TacticalUpgradeSelectionButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button selectButton;

    private Upgrade upgrade;
    private UpgradePopup popup;

    public void Setup(Upgrade upgrade, UpgradePopup popup, Color color)
    {
        this.upgrade = upgrade;
        this.popup = popup;

        iconImage.sprite = upgrade.icon;
        iconImage.color = color;
        titleText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.GetUpgradeEffectText();
        titleText.color = color;
        descriptionText.color = color;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);
    }

    public void SetupAsSpecialAction(string title, string description, Sprite icon, Color color, UnityAction onClick)
    {
        iconImage.sprite = icon;
        iconImage.color = color;
        titleText.text = title;
        descriptionText.text = description;
        titleText.color = color;
        descriptionText.color = color;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(onClick);
    }

    private void OnClick()
    {
        if (!upgrade.IsActivated)
        {
            upgrade.Activate();
        }

        upgrade.ApplyUpgrade();
        UpgradePopup.Inst.OnUpgradeChosen();
    }
}
