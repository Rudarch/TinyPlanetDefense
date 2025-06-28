using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpReadyButtonController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    public void SetLevelUps(int count)
    {
        var levelUps = count > 0 ? $"+ {count}" : string.Empty;
        counterText.text = levelUps;
    }
}
