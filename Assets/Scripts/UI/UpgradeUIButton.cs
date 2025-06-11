using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpgradeUIButton : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Image background;
    public Sprite backgroundEnabled;
    public Sprite backgroundDisabled;
    public UnityEngine.UI.Image icon;
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
                    audioSource.pitch = Random.Range(0.95f, 1.05f); // optional variation
                    audioSource.PlayOneShot(clickSound);
                }

                onClick?.Invoke(upgrade);
            });
        }

        UpdateVisual(upgrade.enabled);
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
