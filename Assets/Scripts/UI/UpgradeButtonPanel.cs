using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtonPanel : MonoBehaviour
{
    public RectTransform panel;
    public GameObject upgradeButtonPrefab;
    public float horizontalSpacing = 10f;
    public float verticalSpacing = 10f;
    public float maxRowWidthRatio = 1f;

    private readonly List<UpgradeUIButton> buttons = new();
    private readonly Dictionary<Upgrade, UpgradeUIButton> upgradeToButton = new();

    public static UpgradeButtonPanel Inst { get; private set; }

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }

    public void AddUpgradeButton(Upgrade upgrade)
    {
        if (!upgrade.activatable || upgradeToButton.ContainsKey(upgrade))
            return;

        GameObject buttonGO = Instantiate(upgradeButtonPrefab, panel);
        var buttonUI = buttonGO.GetComponent<UpgradeUIButton>();
        if (buttonUI != null)
        {
            buttonUI.Initialize(upgrade, ToggleUpgrade);
            buttons.Add(buttonUI);
            upgradeToButton[upgrade] = buttonUI;
            RebuildLayout();
        }
    }

    public void RemoveUpgradeButton(Upgrade upgrade)
    {
        if (!upgradeToButton.TryGetValue(upgrade, out var button))
            return;

        buttons.Remove(button);
        upgradeToButton.Remove(upgrade);
        Destroy(button.gameObject);
        RebuildLayout();
    }

    public void ResetButtons()
    {
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }

        buttons.Clear();
        upgradeToButton.Clear();
    }

    public void RebuildLayout()
    {
        float maxRowWidth = panel.rect.width * maxRowWidthRatio;
        Vector2 buttonSize = upgradeButtonPrefab.GetComponent<RectTransform>().sizeDelta;

        float x = 0f;
        float y = 0f;

        foreach (var button in buttons)
        {
            RectTransform rt = button.GetComponent<RectTransform>();
            if (x + buttonSize.x > maxRowWidth && x > 0f)
            {
                x = 0f;
                y -= buttonSize.y + verticalSpacing;
            }

            rt.anchoredPosition = new Vector2(x, y);
            x += buttonSize.x + horizontalSpacing;
        }
    }

    public UpgradeUIButton GetButtonForUpgrade(Upgrade upgrade)
    {
        upgradeToButton.TryGetValue(upgrade, out var button);
        return button;
    }

    private void ToggleUpgrade(Upgrade upgrade)
    {
        Upgrades.Inst.ToggleUpgrade(upgrade);
        GetButtonForUpgrade(upgrade)?.UpdateVisual(upgrade.enabled);
    }
}
