using UnityEngine;

public class UpgradeUIButton : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image icon;
    public Sprite backgroundEnabled;
    public Sprite backgroundDisabled;
    public GameObject glowEffect;
    public Color disabledColor;

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

    void Update()
    {
        if (upgrade.activationStyle == ActivationStyle.Timed)
        {
            if (glowEffect != null && upgrade.IsReadyForActivation && !glowEffect.activeSelf)
                glowEffect.SetActive(true);
            else if (glowEffect != null && !upgrade.IsReadyForActivation && glowEffect.activeSelf)
                glowEffect.SetActive(false);
        }
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
        UpdateVisual(upgrade.IsActivated);
    }

    public void UpdateVisual(bool isActive)
    {
        if(backgroundEnabled == null || backgroundDisabled == null) return;
        if (isActive)
        {
            background.sprite = backgroundEnabled;
            icon.color = Color.white;
        }
        else
        {

            background.sprite = backgroundDisabled;
            icon.color = disabledColor;
        }
    }
}
