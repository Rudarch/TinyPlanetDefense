
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonPanel : MonoBehaviour
{
    public RectTransform panel;
    public GameObject upgradeButtonPrefab;
    public GameObject rowPrefab;

    private readonly List<RectTransform> rows = new();
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
        if (upgrade.activationStyle == ActivationStyle.Passive || upgradeToButton.ContainsKey(upgrade))
            return;

        RectTransform targetRow = GetOrCreateRowForNextButton();
        GameObject buttonGO = Instantiate(upgradeButtonPrefab, targetRow);
        var buttonUI = buttonGO.GetComponent<UpgradeUIButton>();
        if (buttonUI != null)
        {
            buttonUI.Initialize(upgrade, Upgrades.Inst.ToggleUpgrade);
            upgradeToButton[upgrade] = buttonUI;
        }
    }

    public void RemoveUpgradeButton(Upgrade upgrade)
    {
        if (!upgradeToButton.TryGetValue(upgrade, out var button))
            return;

        upgradeToButton.Remove(upgrade);
        Destroy(button.gameObject);
        CleanEmptyRows();
    }

    public void ResetButtons()
    {
        foreach (var kv in upgradeToButton)
        {
            if (kv.Value != null)
                Destroy(kv.Value.gameObject);
        }
        upgradeToButton.Clear();

        foreach (var row in rows)
        {
            Destroy(row.gameObject);
        }
        rows.Clear();
    }

    public UpgradeUIButton GetButtonForUpgrade(Upgrade upgrade)
    {
        upgradeToButton.TryGetValue(upgrade, out var button);
        return button;
    }

    private RectTransform GetOrCreateRowForNextButton()
    {
        float buttonWidth = upgradeButtonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        RectTransform currentRow = rows.Count > 0 ? rows[rows.Count - 1] : null;

        if (currentRow == null || !CanFitInRow(currentRow, buttonWidth))
        {
            GameObject newRow = Instantiate(rowPrefab, panel);
            currentRow = newRow.GetComponent<RectTransform>();
            rows.Add(currentRow);
        }

        return currentRow;
    }

    private bool CanFitInRow(RectTransform row, float nextButtonWidth)
    {
        var layout = row.GetComponent<HorizontalLayoutGroup>();
        if (layout == null) return false;

        float totalPadding = layout.padding.left + layout.padding.right;
        float spacing = layout.spacing;
        float rowWidth = row.rect.width;

        float usedWidth = totalPadding;
        foreach (RectTransform child in row)
        {
            usedWidth += child.sizeDelta.x + spacing;
        }

        return usedWidth + nextButtonWidth <= rowWidth;
    }

    private void CleanEmptyRows()
    {
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            if (rows[i] == null || rows[i].childCount > 0) continue;
            Destroy(rows[i].gameObject);
            rows.RemoveAt(i);
        }
    }
}
