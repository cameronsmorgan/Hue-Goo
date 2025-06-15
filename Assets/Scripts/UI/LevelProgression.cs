using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelProgression : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject helpPanel; // Assign in Inspector
    [SerializeField] private Button helpButton; // Assign in Inspector
    [SerializeField] private Button closeHelpButton; // Assign in Inspector
    [SerializeField] private Button doneButton; // Assign in Inspector
    [SerializeField] private Button quitButton; // Assign in Inspector

    [Header("Settings")]
    [SerializeField] private bool showHelpAtStart = true;
    [SerializeField] private string nextSceneName; // Alternative to build index

    private void Start()
    {
        // Set up button listeners
        if (helpButton != null)
            helpButton.onClick.AddListener(ShowHelpPanel);

        if (closeHelpButton != null)
            closeHelpButton.onClick.AddListener(HideHelpPanel);

        if (doneButton != null)
            doneButton.onClick.AddListener(OnDoneButtonClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClicked);

        // Show help panel at start if enabled
        if (showHelpAtStart && helpPanel != null)
        {
            helpPanel.SetActive(true);

            // Optional: Pause game when help is shown
            Time.timeScale = 0f;
        }
        else if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
    }

    // Method to be called when the "Done" button is clicked
    public void OnDoneButtonClicked()
    {
        // Option 1: Load next scene by name (set in Inspector)
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Option 2: Load next scene by build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("All levels completed! Returning to menu.");
            SceneManager.LoadScene(0); // Assuming 0 is your menu scene
        }
    }

    public void ShowHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game
        }
    }

    public void HideHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
            Time.timeScale = 1f; // Resume game
        }
    }

    public void OnQuitButtonClicked()
    {
        // For standalone builds
#if UNITY_STANDALONE
        Application.Quit();
#endif

        // For editor testing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnDestroy()
    {
        // Clean up listeners to prevent memory leaks
        if (helpButton != null)
            helpButton.onClick.RemoveListener(ShowHelpPanel);

        if (closeHelpButton != null)
            closeHelpButton.onClick.RemoveListener(HideHelpPanel);

        if (doneButton != null)
            doneButton.onClick.RemoveListener(OnDoneButtonClicked);

        if (quitButton != null)
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }
}