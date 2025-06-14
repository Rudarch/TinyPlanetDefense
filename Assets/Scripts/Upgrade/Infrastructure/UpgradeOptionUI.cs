using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text energyIncreaseText;
    public TMP_Text currentEnergyCostText;
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
        energyIncreaseText.text = $"-{upgrade.GetEnergyCostIncreaseForNextLevel()}";
        currentEnergyCostText.text = $"-{upgrade.energyCostPerSecond}";
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelected);
    }


    private void OnSelected()
    {
        upgrade.ApplyUpgrade();
        //upgrade.Activate();

        if (upgrade.activatable)
        {
            UpgradeButtonPanel.Inst.AddUpgradeButton(upgrade);
        }

        popup.Hide();
    }
}