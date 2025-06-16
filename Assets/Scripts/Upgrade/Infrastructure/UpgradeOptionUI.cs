using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text energyCostText;
    public Button selectButton;

    private Upgrade upgrade;
    private UpgradePopup popup;

    public void Setup(Upgrade upgrade, UpgradePopup popup)
    {
        this.upgrade = upgrade;
        this.popup = popup;

        icon.sprite = upgrade.icon;
        nameText.text = upgrade.upgradeName;
        descriptionText.text = $"{upgrade.description}\n<size=80%><color=yellow>{upgrade.GetUpgradeEffectText()}</color></size>";

        switch (upgrade.activationStyle)
        {
            case ActivationStyle.Timed:
                energyCostText.text = $"-{upgrade.GetEnergyActivationCostForNextLevel()} energy per activation";
                break;
            case ActivationStyle.Toggle:
                energyCostText.text = $"-{upgrade.GetEnergyDrainForNextLevel()} energy per second";
                break;
            default:
                energyCostText.text = $"Passive";
                break;
        }
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelected);
    }


    private void OnSelected()
    {
        upgrade.ApplyUpgrade();

        if (upgrade.activationStyle != ActivationStyle.Passive)
        {
            UpgradeButtonPanel.Inst.AddUpgradeButton(upgrade);
        }
        else
        {
            upgrade.Activate();
        }
        
        popup.Hide();
    }
}