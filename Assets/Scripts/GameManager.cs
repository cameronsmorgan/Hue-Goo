using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public Text player1ScoreText;
    public Text player2ScoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AwardPoint(string playerTag)
    {
        if (playerTag == "Player1")
        {
            player1Score++;
            Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (playerTag == "Player2")
        {
            player2Score++;
            Debug.Log("Player 2 Score: " + player2Score);
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = "Player 1: " + player1Score;

        if (player2ScoreText != null)
            player2ScoreText.text = "Player 2: " + player2Score;
    }
}
