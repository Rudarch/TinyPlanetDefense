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

    private float targetFillAmount = 0f;
    private Coroutine flashRoutine;

    [Header("Pulse Effect")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.15f;

    private Coroutine pulseRoutine;

    [Header("Gameplay Settings")]
    public float bonusDuration = 10f; // how long bonus lasts
    public float bonusIncreasePerSuccess = 0.1f; // 10% per correct press
    public float decayRate = 0.05f; // per second
    public float regenMultiplier = 1.3f; // 30% increased regen

    private float bonusFillAmount = 0f;
    private bool isLeftCorrect;
    private bool isActive = false;

    void Start()
    {
        leftButton.onClick.AddListener(() => PressedButton(true));
        rightButton.onClick.AddListener(() => PressedButton(false));

        SetNewTarget();

        bonusFillBar.color = fillNormalColor;
        bonusFillBar.fillAmount = 0f;
        targetFillAmount = 0f;
    }
    void Update()
    {
        // Decay logic
        if (bonusFillAmount > 0f)
        {
            bonusFillAmount -= decayRate * Time.deltaTime;
            bonusFillAmount = Mathf.Max(bonusFillAmount, 0f);
        }

        // Apply lerped fill
        targetFillAmount = bonusFillAmount;
        bonusFillBar.fillAmount = Mathf.Lerp(bonusFillBar.fillAmount, targetFillAmount, Time.deltaTime * fillLerpSpeed);

        // Update regen state
        if (bonusFillAmount <= 0f)
            EnergySystem.Inst.SetRegenMultiplier(1f);

        // Auto-activate if bar is low
        if (!isActive && bonusFillAmount < 0.5f)
        {
            ActivateMiniGame();
        }

        // Auto-deactivate if bar is full
        if (isActive && bonusFillAmount >= 1f)
        {
            DeactivateMiniGame();
        }
    }



    public void ActivateMiniGame()
    {
        isActive = true;
        SetNewTarget();
        leftButton.interactable = isActive;
        rightButton.interactable = isActive;
    }

    public void DeactivateMiniGame()
    {
        isActive = false;
        leftPulseIcon.color = Color.white;
        rightPulseIcon.color = Color.white;
        EnergySystem.Inst.SetRegenMultiplier(1f); // reset regen boost
        leftButton.interactable = isActive;
        rightButton.interactable = isActive;
    }

    void SetNewTarget()
    {
        isLeftCorrect = Random.value > 0.5f;
        leftPulseIcon.color = isLeftCorrect ? highlightColor : defaultColor;
        rightPulseIcon.color = isLeftCorrect ? defaultColor : highlightColor;
    }

    void PressedButton(bool left)
    {
        if (!isActive) return;

        if (left == isLeftCorrect)
        {
            bonusFillAmount += bonusIncreasePerSuccess;
            bonusFillAmount = Mathf.Clamp01(bonusFillAmount);
            targetFillAmount = bonusFillAmount;

            if (bonusFillAmount > 0f)
                EnergySystem.Inst.SetRegenMultiplier(regenMultiplier);

            if (pulseRoutine != null) StopCoroutine(pulseRoutine);
            pulseRoutine = StartCoroutine(PlayPulse());
        }
        else
        {
            bonusFillAmount -= bonusIncreasePerSuccess;
            bonusFillAmount = Mathf.Clamp01(bonusFillAmount);
            targetFillAmount = bonusFillAmount;

            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashPenaltyColor());

            if (bonusFillAmount <= 0f)
                EnergySystem.Inst.SetRegenMultiplier(1f);
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
        RectTransform rt = bonusFillBar.rectTransform;
        Vector3 originalScale = rt.localScale;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * pulseScale, originalScale.z);

        float timer = 0f;
        while (timer < pulseDuration)
        {
            float t = timer / pulseDuration;
            rt.localScale = Vector3.Lerp(originalScale, targetScale, t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < pulseDuration)
        {
            float t = timer / pulseDuration;
            rt.localScale = Vector3.Lerp(targetScale, originalScale, t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        rt.localScale = originalScale;
    }


}
