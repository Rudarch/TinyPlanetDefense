using UnityEngine;

public class UIEffectsController : MonoBehaviour
{
    public static UIEffectsController Inst { get; private set; }

    [Header("Optional Blur Effect Reference")]
    public Material blurMaterial;
    private float blurAmount = 0f;
    private string currentSource = null;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }

    void Update()
    {
        if (blurMaterial != null)
        {
            blurMaterial.SetFloat("_Size", blurAmount);
        }
    }

    public void ApplyBlur(string source, float amount)
    {
        if (blurMaterial == null) return;

        Debug.Log($"Blur {source} {amount}");
        if (string.IsNullOrEmpty(currentSource) || source == currentSource)
        {
            blurAmount = Mathf.Max(blurAmount, amount);
            currentSource = source;
        }
    }

    public void ClearBlur(string source)
    {
        if (currentSource == source)
        {
            blurAmount = 0f;
            currentSource = null;
        }
    }
}