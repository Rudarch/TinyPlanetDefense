
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WaveProgressUI : MonoBehaviour
{
    public GameObject waveNumberPrefab;
    public RectTransform waveScrollContent;
    public ScrollRect scrollRect;

    public Color pendingColor = Color.gray;
    public Color activeColor = Color.cyan;
    public Color completedColor = new Color(1f, 0.5f, 0f);

    private List<GameObject> waveItems = new();
    private List<TextMeshProUGUI> waveLabels = new();
    private List<Image> waveIcons = new();
    private int currentWave = 0;
    private Coroutine scrollRoutine;

    public float slideSpeed = 8f;

    public void Initialize(int totalWaves)
    {
        foreach (Transform child in waveScrollContent)
            Destroy(child.gameObject);

        waveItems.Clear();
        waveLabels.Clear();
        waveIcons.Clear();

        for (int i = 0; i < totalWaves; i++)
        {
            GameObject go = Instantiate(waveNumberPrefab, waveScrollContent);
            var label = go.GetComponentInChildren<TextMeshProUGUI>();
            var icon = go.GetComponentInChildren<Image>();

            if (label != null && icon != null)
            {
                label.text = (i + 1).ToString();
                label.color = pendingColor;
                icon.color = pendingColor;

                waveItems.Add(go);
                waveLabels.Add(label);
                waveIcons.Add(icon);
            }
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(waveScrollContent);
        SetWave(0);
    }

    public void SetWave(int waveIndex)
    {
        if (waveIndex >= waveLabels.Count)
            return;

        currentWave = waveIndex;

        for (int i = 0; i < waveLabels.Count; i++)
        {
            Color c = i < currentWave ? completedColor :
                      i == currentWave ? activeColor : pendingColor;

            waveLabels[i].color = c;
            waveIcons[i].color = c;
        }

        if (scrollRoutine != null) StopCoroutine(scrollRoutine);
        scrollRoutine = StartCoroutine(SmoothScrollToIndex(currentWave));
    }

    IEnumerator SmoothScrollToIndex(int index)
    {
        yield return new WaitForEndOfFrame();

        RectTransform item = waveItems[index].GetComponent<RectTransform>();
        Vector2 itemPos = (Vector2)scrollRect.transform.InverseTransformPoint(item.position);
        Vector2 centerPos = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.viewport.position);

        float diff = itemPos.x - centerPos.x;
        Vector2 start = waveScrollContent.anchoredPosition;
        Vector2 target = start - new Vector2(diff, 0);

        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            waveScrollContent.anchoredPosition = Vector2.Lerp(start, target, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        waveScrollContent.anchoredPosition = target;
    }

    public void SetEnemiesRemaining(int count)
    {
        if (count <= 0 && currentWave + 1 < waveLabels.Count)
        {
            SetWave(currentWave + 1);
        }
    }
}
