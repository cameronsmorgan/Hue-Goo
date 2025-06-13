using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
            player1Text.text = $"P1: {percentage * 100f:F1}%";
        }
        else if (playerName == "Player2")
        {
            player2Slider.value = percentage;
            player2Text.text = $"P2: {percentage * 100f:F1}%";
        }
    }

    public void ShowWinner(string playerName)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = $"{playerName} finished first!";
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
        winnerText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        finalResultText.gameObject.SetActive(false);
    }
}
