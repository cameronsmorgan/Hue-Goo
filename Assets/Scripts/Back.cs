using UnityEngine;
using UnityEngine.SceneManagement;
public class Back : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject); 
    }

    public void PartyMode()
    {
        SceneManager.LoadScene("PartyMode");
    }
}
