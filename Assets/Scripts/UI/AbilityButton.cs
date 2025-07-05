using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Image glowEffect;

    [Header("Colours")]
    public Color activatedIconColor;
    public Color activatedGlowColor;
    public Color readyColor;
    public Color onCooldownColor;

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

        upgrade.OnUpgradeChanged += UpdateVisual;
        upgrade.OnActivationTimerChanged += UpdateVisualsOnActivationTimeChanged;
        UpdateVisual();
        UpdateVisualsOnActivationTimeChanged(1, 0);
    }

    public void UpdateVisualsOnActivationTimeChanged(float max, float value)
    {
        glowEffect.fillAmount = value / max;
    }

    public void UpdateVisual()
    {
        if (BoundUpgrade.IsActivated)
        {
            icon.color = activatedIconColor;
            glowEffect.color = activatedGlowColor;
        }
        else if (BoundUpgrade.IsReadyForActivation)
        {
            icon.color = readyColor;
            glowEffect.color = readyColor;

            UpdateVisualsOnActivationTimeChanged(1, 1);
            coloroutine?.StartColorTransition();
            return;
        }
        else
        {
            icon.color = onCooldownColor;
            glowEffect.color = onCooldownColor;
            UpdateVisualsOnActivationTimeChanged(1, 1);
        }

        coloroutine?.StopColorTransition();
    }
}
