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

    public bool isFirstRound = true;

    public List<string> raceRounds;
    public List<int> roundWinners = new List<int>(3);

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

        if (Instance.roundWinners.Count == 0)
            Instance.roundWinners = new List<int> { 0, 0, 0 };

        if (Instance.raceRounds.Count == 0)
        {
            populateRaceRounds(3);
            //LoadNextRoundScene();
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
        

        Debug.Log(Instance.currentRound);
        if (playerName == "Player1")
        {
            player1Score++;
            Instance.roundWinners[Instance.currentRound] = 1;
            Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (playerName == "Player2")
        {
            player2Score++;
            Instance.roundWinners[Instance.currentRound] = 2;
            Debug.Log("Player 2 Score: " + player2Score);
        }

        foreach (int winner in Instance.roundWinners)
            Debug.Log($"Winner: {winner}");

        Debug.Log($"Hue: {player1Score} - {player2Score} :Goo");
    }

    public void EndRound()
    {
        Debug.Log("End Round");

        currentRound++;

        Debug.Log($"Next ROund = : {currentRound}");

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in taggedObjects)
        {
            obj.GetComponent<PlayerMovement>().canMove = false;
        }

        if (currentRound >= totalRounds)
        {
            string result;

            if (player1Score > player2Score)
            {
                result = "Player 1 Wins!";
                PartyModeManager.lastRoundWinner = 1;
                SceneManager.LoadScene("UltimateWinner");
            }
            else if (player2Score > player1Score)
            {
                result = "Player 2 Wins!";
                PartyModeManager.lastRoundWinner = 2;
                SceneManager.LoadScene("UltimateWinner");
            }
            else
            {
                result = "It's a Tie!";
                PartyModeManager.lastRoundWinner = 3;
                SceneManager.LoadScene("UltimateWinner");
            }

            // Optional: Show result before loading if desired
            Debug.Log(result);
        }
        else
        {
            LoadNextRoundScene();
        }
    }


    public void LoadNextRoundScene()
    {
        //string[] roundScenes = { "Race1", "Race2", "Race3" };
        Debug.Log("load");
        if (currentRound < raceRounds.Count)
        {
            string nextSceneName = raceRounds[currentRound];
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void populateRaceRounds(int n)
    {
        raceRounds = new List<string>();
        List<string> sceneNames = new List<string> {"Race1", "Race2", "Race3", "Race4", "Race5", "Race6" };
        while (raceRounds.Count != n)
        {
            int randomIndex = Random.Range(0, sceneNames.Count);
            string sceneName = sceneNames[randomIndex];
            if (!raceRounds.Contains(sceneName))
            {
                raceRounds.Add(sceneName);
            }
        }

        foreach (var scene in raceRounds)
        {
            Debug.Log("Lineup: " + scene);
        }


    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        currentRound = 0;
    }


}
