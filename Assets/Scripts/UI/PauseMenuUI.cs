using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject helpPanelUI;

    private bool isPaused = false;

    public void OpenPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Home");
    }

    public void OpenHelpPanel()
    {
        helpPanelUI.SetActive(true);
        pauseMenuUI.SetActive(false); // Hide settings
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void CloseHelpPanel()
    {
        helpPanelUI.SetActive(false);
        if (isPaused)
        {
            pauseMenuUI.SetActive(true); // Show settings again if game is paused
        }
    }
}
