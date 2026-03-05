using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuRoot; // drag PauseMenu here

    private bool isPaused = false;

    private void Start()
    {
        // Ensure game is unpaused when scene starts
        ResumeGame();
    }

    private void Update()
    {
        // ESC sometimes gets swallowed in WebGL, so P is a backup
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuRoot != null)
            pauseMenuRoot.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuRoot != null)
            pauseMenuRoot.SetActive(false);
    }

    // Hooking this up now, scene can be added later
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // create mainmenu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_WEBGL
        // Browsers can't quit apps — treat as "back to menu" or show message
        SceneManager.LoadScene("MainMenu");
#else
        Application.Quit();
#endif
    }
}
