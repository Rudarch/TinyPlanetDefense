using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public Button retryButton;
    private int score;

    void Start()
    {
        gameOverPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOver(int finalScore)
    {
        score = finalScore;
        gameOverPanel.SetActive(true);
        scoreText.text = "Score: " + score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}