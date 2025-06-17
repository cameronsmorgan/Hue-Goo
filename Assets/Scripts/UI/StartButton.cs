using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Call this method from your Play Button's OnClick()
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
