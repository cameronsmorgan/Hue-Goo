using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Needed to load scenes

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public Text player1ScoreText;
    public Text player2ScoreText;

    [Header("Audio")]
    public AudioClip flowerCollectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        UpdateScoreUI();

        // ✅ Subscribe to timer end event
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer != null)
        {
            timer.OnTimerEnd += EvaluateFlowerWinner;
        }
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

        PlayFlowerSound();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = player1Score + " - Hue";

        if (player2ScoreText != null)
            player2ScoreText.text = "Goo - " + player2Score;
    }

    private void PlayFlowerSound()
    {
        if (flowerCollectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(flowerCollectSound);
        }
    }

    //Win logic when timer ends
    private void EvaluateFlowerWinner()
    {
        if (player1Score > player2Score)
        {
            Debug.Log("Player 1 Wins!");
            PartyModeManager.lastRoundWinner = 1; 
            SceneManager.LoadScene("UltimateWinner");
        }
        else if (player2Score > player1Score)
        {
            Debug.Log("Player 2 Wins!");
            PartyModeManager.lastRoundWinner = 2;
            SceneManager.LoadScene("UltimateWinner");
        }
        else
        {
            Debug.Log("It's a tie — showing random winner.");
            PartyModeManager.lastRoundWinner = 3;
            SceneManager.LoadScene("UltimateWinner");
        }
    }
}
