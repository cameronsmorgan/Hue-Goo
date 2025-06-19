using UnityEngine;
using System.Collections;
public class RaceLevelManager : MonoBehaviour
{
    public static RaceLevelManager Instance;

    public PlayerColorFill player1Fill;
    public PlayerColorFill player2Fill;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player1Fill.ResetForNextRound();
        player2Fill.ResetForNextRound();

        if (RaceUIManager.Instance != null)
        {
            RaceUIManager.Instance.HideAllUI();
        }
    }

    public void PlayerFinished(string playerName)
    {
        Debug.Log(playerName + " finished!");

        

        // Notify GameManager
        Level4GameManager.Instance.PlayerFinished(playerName);
    }
}
