using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TMP_Text waveReachedText;

    public WaveManager waveManager;

    private bool gameOver = false;

    public void TriggerGameOver()
    {
        if (gameOver) return;

        gameOver = true;

        Time.timeScale = 0f;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        if (waveManager != null && waveReachedText != null)
            waveReachedText.text = $"You reached Wave {waveManager.CurrentWave}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}