using UnityEngine;

public class UpgradeUIButton : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image icon;
    public Sprite backgroundEnabled;
    public Sprite backgroundDisabled;
    public UnityEngine.UI.Image glowEffect;
    public Color disabledColor;
    public Color toggleOnColor;
    public Color timedOnColor;
    public Color enoughEnergyForActivationColor;

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

        if (upgrade.activationStyle == ActivationStyle.Timed)
        {
            EnergySystem.OnEnergyChanged += HandleEnergyChanged;
        }

        upgrade.OnActivationChanged += UpdateVisual;
        upgrade.OnActivationTimerChanged += UpdateGlow;
        UpdateVisual(upgrade.IsActivated);
        UpdateGlow(1, 0);
    }

    void HandleEnergyChanged(float currentEnergy, float maxEnergy)
    {
        if (upgrade.IsActivated) return;

        if (currentEnergy >= upgrade.activationEnergyAmount)
            icon.color = enoughEnergyForActivationColor;
        else 
            icon.color = disabledColor;
    }

    public void UpdateGlow(float max, float value)
    {
        glowEffect.fillAmount = value / max;
    }

    public void UpdateVisual(bool isActive)
    {
        if(backgroundEnabled == null || backgroundDisabled == null) return;

        if (isActive)
        {
            background.sprite = backgroundEnabled;
            icon.color = Color.white;

            if (upgrade.activationStyle == ActivationStyle.Toggle)
            {
                UpdateGlow(1, 1);
                glowEffect.color = toggleOnColor;
            }
            else
            {
                glowEffect.color = timedOnColor;
            }
        }
        else
        {
            background.sprite = backgroundDisabled;
            icon.color = disabledColor;

            if (upgrade.activationStyle == ActivationStyle.Toggle)
            {
                UpdateGlow(1, 1);
                glowEffect.color = disabledColor;
            }
        }
    }
}
