using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4GameManager : MonoBehaviour
{
    public static Level4GameManager Instance;

    public int player1Score = 0;
    public int player2Score = 0;

    public int currentRound = 0;
    public int totalRounds = 3;
    public float countdownDuration = 3f;

    private bool roundOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Called every time a new scene loads
        roundOver = false;

        // Disable all Race UI texts on new scene load
        if (RaceUIManager.Instance != null)
        {
            RaceUIManager.Instance.HideAllUI();
        }
    }

    public void PlayerFinished(string playerName)
    {
        if (roundOver) return;
        roundOver = true;

        AwardPoint(playerName);

        RaceUIManager.Instance.ShowWinner(playerName);
        RaceUIManager.Instance.StartCountdown(countdownDuration);
    }

    public void AwardPoint(string playerName)
    {
        if (playerName == "Player1")
        {
            player1Score++;
            Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (playerName == "Player2")
        {
            player2Score++;
            Debug.Log("Player 2 Score: " + player2Score);
        }
    }

    public void EndRound()
    {
        currentRound++;

        if (currentRound >= totalRounds)
        {
            string result;

            if (player1Score > player2Score)
                result = "Player 1 Wins!";
            else if (player2Score > player1Score)
                result = "Player 2 Wins!";
            else
                result = "It's a Tie!";

            RaceUIManager.Instance.ShowFinalResult(result);
        }
        else
        {
            LoadNextRoundScene();
        }
    }

    public void LoadNextRoundScene()
    {
        string[] roundScenes = { "Level4", "Level4.1", "Level4.2" };

        if (currentRound < roundScenes.Length)
        {
            string nextSceneName = roundScenes[currentRound];
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        currentRound = 0;
    }
}
