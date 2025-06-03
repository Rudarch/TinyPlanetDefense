using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMobileController : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene("GameScene");

    public void OpenUpgrades() => SceneManager.LoadScene("UpgradeScene");

    public void OpenSettings() => Debug.Log("Settings not implemented yet");

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
