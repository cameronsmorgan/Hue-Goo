using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void load()
    {
        SceneManager.LoadScene("PartyMode");
    }
}
