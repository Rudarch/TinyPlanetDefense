using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EnergyPulseMiniGame : MonoBehaviour
{
    [Header("UI Elements")]
    public Button leftButton;
    public Button rightButton;
    public Image leftPulseIcon;
    public Image rightPulseIcon;
    public Image bonusFillBar;
    public TextMeshProUGUI bonusText;

    [Header("Colors")]
    public Color highlightColor = Color.magenta;
    public Color defaultColor = Color.white;

    [Header("Visual Feedback")]
    public Color fillNormalColor = Color.magenta;
    public Color fillPenaltyColor = Color.red;
    public float fillLerpSpeed = 5f;
    public float penaltyFlashDuration = 0.2f;

    [Header("Pulse Effect")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.15f;

    [Header("Heat System")]
    public float energyGainPerSuccess = 8f;
    public float heatIncreasePerPress = 0.15f;
    public float heatDecayRate = 0.05f;
    public float overheatCooldownThreshold = 0.7f;

    [Header("Feedback UI")]
    public TextMeshProUGUI feedbackLabel;
    public CanvasGroup feedbackCanvas;
    public float feedbackDuration = 1.2f;
    public Color energyGainColor = Color.white;
    public Color overheatColor = Color.red;
    public Color wrongPressColor = Color.yellow;

    [Header("Bonus energy")]
    public float timeSinceLastCorrectPressThreshhold = 1f;

    private Vector3 bonusFillBarOriginalScale;
    private float currentHeat = 0f;
    private bool isLeftCorrect;
    private bool isActive = false;
    private bool isOverheated = false;
    private Coroutine flashRoutine;
    private Coroutine pulseRoutine;
    private Coroutine feedbackRoutine;
    private int correctStreak = 0; 
    private float timeSinceLastCorrectPress = 0f;

    void Start()
    {
        leftButton.onClick.AddListener(() => PressedButton(true));
        rightButton.onClick.AddListener(() => PressedButton(false));
        bonusFillBarOriginalScale = bonusFillBar.rectTransform.localScale;

        isLeftCorrect = Random.value > 0.5f;
        SetNewTarget();
        bonusFillBar.color = fillNormalColor;
        bonusFillBar.fillAmount = 0f;
    }

    void Update()
    {
        if (currentHeat > 0f)
        {
            currentHeat -= heatDecayRate * Time.deltaTime;
            currentHeat = Mathf.Clamp01(currentHeat);
        }

        if (!isOverheated && isActive)
        {
            timeSinceLastCorrectPress += Time.deltaTime;
            if (timeSinceLastCorrectPress > timeSinceLastCorrectPressThreshhold)
            {
                correctStreak = 0;
                timeSinceLastCorrectPress = 0f;
            }
        }

        bonusFillBar.fillAmount = Mathf.Lerp(bonusFillBar.fillAmount, currentHeat, Time.deltaTime * fillLerpSpeed);

        if (!isOverheated && !isActive)
            ActivateMiniGame();

        if (isOverheated && currentHeat <= overheatCooldownThreshold)
        {
            isOverheated = false;
            ActivateMiniGame();
        }
    }

    public void ActivateMiniGame()
    {
        isActive = true;

        isLeftCorrect = Random.value > 0.5f;
        SetNewTarget();
        leftButton.interactable = true;
        rightButton.interactable = true;
        leftPulseIcon.color = isLeftCorrect ? highlightColor : defaultColor;
        rightPulseIcon.color = isLeftCorrect ? defaultColor : highlightColor;
    }

    public void DeactivateMiniGame()
    {
        isActive = false;
        leftPulseIcon.color = Color.gray;
        rightPulseIcon.color = Color.gray;
        leftButton.interactable = false;
        rightButton.interactable = false;
    }

    void SetNewTarget()
    {
        isLeftCorrect = !isLeftCorrect;

        if (!isOverheated)
        {
            leftPulseIcon.color = isLeftCorrect ? highlightColor : defaultColor;
            rightPulseIcon.color = isLeftCorrect ? defaultColor : highlightColor;
        }
    }

    void PressedButton(bool left)
    {
        if (!isActive || isOverheated) return;

        if (left == isLeftCorrect)
        {
            timeSinceLastCorrectPress = 0f;

            float bonusEnergy = energyGainPerSuccess + correctStreak + Upgrades.Inst.EnergyMatrix.BonusPulseTap;
            EnergySystem.Inst.Restore(bonusEnergy);
            correctStreak++;

            currentHeat += heatIncreasePerPress;
            currentHeat = Mathf.Clamp01(currentHeat);

            if (feedbackRoutine != null) StopCoroutine(feedbackRoutine);
            feedbackRoutine = StartCoroutine(ShowFeedback($"+{bonusEnergy:F0} Energy", energyGainColor));

            if (currentHeat >= 1f)
            {
                isOverheated = true;
                correctStreak = 0;
                DeactivateMiniGame();
                feedbackRoutine = StartCoroutine(ShowFeedback("OVERHEATED!", overheatColor));
            }

            if (pulseRoutine != null) StopCoroutine(pulseRoutine);
            pulseRoutine = StartCoroutine(PlayPulse());
        }
        else
        {
            correctStreak = 0;

            currentHeat += heatIncreasePerPress;
            currentHeat = Mathf.Clamp01(currentHeat);

            string[] missMessages = { "Oops!", "Miss!", "Wrong!", "Try Again!" };
            string msg = missMessages[Random.Range(0, missMessages.Length)];
            feedbackRoutine = StartCoroutine(ShowFeedback(msg, wrongPressColor));

            if (currentHeat >= 1f)
            {
                isOverheated = true;
                DeactivateMiniGame();
                if (feedbackRoutine != null) StopCoroutine(feedbackRoutine);
                feedbackRoutine = StartCoroutine(ShowFeedback("OVERHEATED!", overheatColor));
            }

            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashPenaltyColor());
        }

        SetNewTarget();
    }

    IEnumerator FlashPenaltyColor()
    {
        bonusFillBar.color = fillPenaltyColor;
        yield return new WaitForSeconds(penaltyFlashDuration);
        bonusFillBar.color = fillNormalColor;
    }

    IEnumerator PlayPulse()
    {
        Vector3 targetScale = new Vector3(bonusFillBarOriginalScale.x, bonusFillBarOriginalScale.y * pulseScale, bonusFillBarOriginalScale.z);

        float timer = 0f;
        while (timer < pulseDuration)
        {
            float t = timer / pulseDuration;
            bonusFillBar.rectTransform.localScale = Vector3.Lerp(bonusFillBarOriginalScale, targetScale, t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < pulseDuration)
        {
            float t = timer / pulseDuration;
            bonusFillBar.rectTransform.localScale = Vector3.Lerp(targetScale, bonusFillBarOriginalScale, t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        bonusFillBar.rectTransform.localScale = bonusFillBarOriginalScale;
    }

    IEnumerator ShowFeedback(string message, Color color)
    {
        feedbackLabel.text = message;
        feedbackLabel.color = color;
        feedbackCanvas.alpha = 1f;

        float t = 0f;
        while (t < feedbackDuration)
        {
            t += Time.deltaTime;
            feedbackCanvas.alpha = Mathf.Lerp(1f, 0f, t / feedbackDuration);
            yield return null;
        }

        feedbackLabel.text = "";
        feedbackCanvas.alpha = 0f;
    }
}