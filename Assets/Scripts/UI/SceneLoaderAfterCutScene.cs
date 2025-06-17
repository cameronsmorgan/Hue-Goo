using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderAfterCutscene : MonoBehaviour
{
    public static string nextSceneName;

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
