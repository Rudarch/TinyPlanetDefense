using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtonPanel : MonoBehaviour
{
    public RectTransform panel;
    public GameObject upgradeButtonPrefab;
    public float horizontalSpacing = 10f;
    public float verticalSpacing = 10f;
    public float maxRowWidthRatio = 0.9f;

    private readonly List<GameObject> buttons = new();
    private readonly HashSet<System.Type> activeUpgradeTypes = new();
    public static UpgradeButtonPanel Inst { get; private set; }
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(this);
            return;
        }
        Inst = this;
    }

    public void AddUpgradeButton(Upgrade upgrade)
    {
        if (!upgrade.activatable)
            return;

        var upgradeType = upgrade.GetType();

        if (activeUpgradeTypes.Contains(upgradeType))
            return;

        GameObject buttonGO = Instantiate(upgradeButtonPrefab, panel);
        var button = buttonGO.GetComponent<UpgradeUIButton>();
        if (button != null)
        {
            button.Initialize(upgrade, (upg) => {
                Upgrades.Inst.ToggleUpgrade(upg);
                button.SetEnabled(!upgrade.enabled);
            });
        }

        buttons.Add(buttonGO);
        activeUpgradeTypes.Add(upgradeType);
        RebuildLayout();
    }

    public void RemoveUpgradeButton(Upgrade upgrade)
    {
        GameObject target = buttons.Find(go =>
        {
            var btn = go.GetComponent<UpgradeUIButton>();
            return btn != null && btn.GetUpgrade() == upgrade;
        });

        if (target != null)
        {
            buttons.Remove(target);
            Destroy(target);
            RebuildLayout();
        }
    }

    public void ResetButtons()
    {
        foreach (var button in buttons)
            Destroy(button);

        activeUpgradeTypes.Clear();
    }

    private void RebuildLayout()
    {
        float maxRowWidth = panel.rect.width * maxRowWidthRatio;
        Vector2 buttonSize = upgradeButtonPrefab.GetComponent<RectTransform>().sizeDelta;

        float x = 0f, y = 0f;
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform rt = buttons[i].GetComponent<RectTransform>();
            if (x + buttonSize.x > maxRowWidth && x > 0f)
            {
                x = 0f;
                y -= buttonSize.y + verticalSpacing;
            }

            rt.anchoredPosition = new Vector2(x, y);
            x += buttonSize.x + horizontalSpacing;
        }
    }
}