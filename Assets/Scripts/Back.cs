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
        PartyModeManager.lastRoundWinner = Random.Range(1, 3); // Returns 1 or 2
        SceneManager.LoadScene("UltimateWinner");

    }
}
