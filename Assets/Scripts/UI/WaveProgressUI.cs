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

    private int displayOffset = 0;
    private const int maxVisible = 5;
    private const int bufferSize = 7;

    public float slideSpeed = 8f;

    void Start()
    {
        for (int i = 0; i < bufferSize; i++)
        {
            GameObject go = Instantiate(waveNumberPrefab, waveScrollContent);
            var label = go.GetComponentInChildren<TextMeshProUGUI>();
            var icon = go.GetComponentInChildren<Image>();

            int waveNumber = i + 1;

            label.text = waveNumber.ToString();
            label.color = pendingColor;
            icon.color = pendingColor;

            waveItems.Add(go);
            waveLabels.Add(label);
            waveIcons.Add(icon);
        }

        SetWave(0);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(waveScrollContent);
    }

    public void SetWave(int waveIndex)
    {
        currentWave = waveIndex;

        if (waveIndex > displayOffset + 3)
        {
            RemoveLeftMostWave();
            AddNewWaveAtEnd(waveIndex + 3);
            displayOffset++;
        }

        for (int i = 0; i < waveLabels.Count; i++)
        {
            int waveNum = displayOffset + i;
            Color c = waveNum < currentWave ? completedColor :
                      waveNum == currentWave ? activeColor : pendingColor;

            waveLabels[i].color = c;
            waveIcons[i].color = c;

            waveItems[i].transform.localScale = (waveNum == currentWave)
                ? Vector3.one * 1.5f
                : Vector3.one;
        }

        if (scrollRoutine != null) StopCoroutine(scrollRoutine);
        scrollRoutine = StartCoroutine(SmoothScrollToCenter(currentWave - displayOffset));
    }

    void RemoveLeftMostWave()
    {
        Destroy(waveItems[0]);
        waveItems.RemoveAt(0);
        waveLabels.RemoveAt(0);
        waveIcons.RemoveAt(0);
    }

    void AddNewWaveAtEnd(int waveNumber)
    {
        GameObject go = Instantiate(waveNumberPrefab, waveScrollContent);
        var label = go.GetComponentInChildren<TextMeshProUGUI>();
        var icon = go.GetComponentInChildren<Image>();

        label.text = (waveNumber + 1).ToString();
        label.color = pendingColor;
        icon.color = pendingColor;

        waveItems.Add(go);
        waveLabels.Add(label);
        waveIcons.Add(icon);
    }

    IEnumerator SmoothScrollToCenter(int localIndex)
    {
        yield return new WaitForEndOfFrame();

        if (waveItems.Count == 0 || localIndex >= waveItems.Count) yield break;

        RectTransform item = waveItems[localIndex].GetComponent<RectTransform>();
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
    void OnEnable()
    {
        WaveEvents.OnWaveStarted += SetWave;
    }

    void OnDisable()
    {
        WaveEvents.OnWaveStarted -= SetWave;
    }
    public void SetEnemiesRemaining(int count)
    {
        if (count <= 0)
        {
            SetWave(currentWave + 1);
        }
    }
}