using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Image glowEffect;

    [Header("Toggle Colours")]
    public Color toggleOnIconColor;
    public Color toggleOnGlowColor;
    public Color disabledToggleIconColor;
    public Color disabledToggleGlowColor;

    [Header("Timed Colours")]
    public Color timedOnIconColor;
    public Color timedOnGlowColor;
    public Color enoughEnergyForActivationIconColor;
    public Color enoughEnergyForActivationGlowColor;
    public Color notEnoughEnergyForActivationIconColor;
    public Color notEnoughEnergyForActivationGlowColor;

    public ColorLerpCoroutine coloroutine;
    public AudioClip clickSound;

    private AudioSource audioSource;
    private Upgrade upgrade;

    public Upgrade BoundUpgrade => upgrade;
    public RectTransform RectTransform => GetComponent<RectTransform>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Initialize(Upgrade upgradeData, System.Action<Upgrade> onClick)
    {
        upgrade = upgradeData;

        if (icon != null && upgrade.icon != null)
            icon.sprite = upgrade.icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (clickSound != null && audioSource != null)
                {
                    audioSource.pitch = Random.Range(0.95f, 1.05f);
                    audioSource.PlayOneShot(clickSound);
                }

                onClick?.Invoke(upgrade);
            });
        }

        upgrade.OnActivationChanged += UpdateVisual;
        upgrade.OnActivationTimerChanged += UpdateGlow;
        UpdateVisual(upgrade.IsActivated);
        UpdateGlow(1, 1);
    }

    public void UpdateGlow(float max, float value)
    {
        glowEffect.fillAmount = value / max;
    }

    public void UpdateVisual(bool isActive)
    {
        if (isActive)
        {
            icon.color = timedOnIconColor;
            glowEffect.color = timedOnGlowColor;
        }
        else
        {
            icon.color = disabledToggleIconColor;
            UpdateGlow(1, 1);
            glowEffect.color = disabledToggleGlowColor;
        }
    }
}
