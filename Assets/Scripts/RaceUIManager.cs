using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RaceUIManager : MonoBehaviour
{
    public static RaceUIManager Instance;

    [Header("Player UI")]
    public Slider player1Slider;
    public Slider player2Slider;
    public Text player1Text;
    public Text player2Text;

    [Header("Round UI")]
    public Text winnerText;
    public Text countdownText;
    public Text finalResultText;

    [Header("Winner Images")]
    public List<Image> roundWinnerImages;
    public Sprite player1Leaf;
    public Sprite player2Leaf;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdatePlayerProgress(string playerName, float percentage)
    {
        if (playerName == "Player1")
        {
            player1Slider.value = percentage;
            player1Text.text = $"{(int) (percentage * 100f)}%";
        }
        else if (playerName == "Player2")
        {
            player2Slider.value = percentage;
            player2Text.text = $"{(int) (percentage * 100f)}%";
        }
    }

    public void ShowWinner(string playerName)
    {
        winnerText.gameObject.SetActive(true);

        if (playerName == "Player1")
            playerName = "Hue";
        if (playerName == "Player2")
            playerName = "Goo";

        winnerText.text = $"{playerName} finished first!";
        ShowRoundWinnerUI();
    }

    public void StartCountdown(float duration)
    {
        StartCoroutine(CountdownRoutine(duration));
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        countdownText.gameObject.SetActive(true);

        float timer = duration;
        while (timer > 0)
        {
            countdownText.text = $"Next round in {Mathf.Ceil(timer)}...";
            timer -= Time.deltaTime;
            yield return null;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        winnerText.gameObject.SetActive(false);

        Level4GameManager.Instance.EndRound();
    }

    public void ShowFinalResult(string result)
    {
        finalResultText.gameObject.SetActive(true);
        finalResultText.text = result;
    }

    public void HideAllUI()
    {
        ShowRoundWinnerUI();

        winnerText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        finalResultText.gameObject.SetActive(false);
    }

    void ShowRoundWinnerUI()
    {
        Debug.Log("Show round winner");
        foreach (int winner in Level4GameManager.Instance.roundWinners)
            Debug.Log($"Winner: {winner}");

        for (int i = 0;i < Level4GameManager.Instance.roundWinners.Count; i++ )
        {
            int winPlayer = Level4GameManager.Instance.roundWinners[i];

            switch (winPlayer)
            {
                case 1:
                    roundWinnerImages[i].sprite = player1Leaf;
                    break;
                case 2:
                    roundWinnerImages[i].sprite = player2Leaf;
                    break;
                default:
                    break;
            }
        }
    }
}
