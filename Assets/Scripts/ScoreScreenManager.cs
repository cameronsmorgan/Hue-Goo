using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; 

public class ScoreScreenManager : MonoBehaviour
{
    [Header("Player 1 stuff")]
    public Transform snailP1;
    public Transform startP1;
    public Transform endP1;

    [Header("Player 2 stuff")]
    public Transform snailP2;
    public Transform startP2;
    public Transform endP2;


    [Header("Animation Stuff")]
    [SerializeField] float moveDuration;

    public Animator GooController;
    public Animator HueController;

    public GameObject button;

    public TMP_Text GooScore;
    public TMP_Text HueScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PartyModeManager.player1WinPoints = 0;
        //PartyModeManager.player2WinPoints = 0;
        //PartyModeManager.numToWin = 3;

        //PartyModeManager.lastRoundWinner = 1;
        SetSnailPositions();

        MoveRoundWinner();
    }

    private void Awake()
    {
        GooController.SetBool("isMoving", false);
        HueController.SetBool("isMoving", false);
    }

    private void Update()
    {
        HueScore.text = $"{PartyModeManager.player1WinPoints}"; 
        GooScore.text = $"{PartyModeManager.player2WinPoints}";

    }

    void SetSnailPositions()
    {
        snailP1.position = Vector3.Lerp(startP1.position, endP1.position, PartyModeManager.player1WinPoints / (float)PartyModeManager.numToWin);
        snailP2.position = Vector3.Lerp(startP2.position, endP2.position, PartyModeManager.player2WinPoints / (float)PartyModeManager.numToWin);
        Debug.Log(snailP1.position); 
    }

   

    void MoveRoundWinner()
    {

        int winner = PartyModeManager.lastRoundWinner;

        if (winner == 0) return;

        if (winner == 1)
            PartyModeManager.player1WinPoints++;
        else if (winner == 2)
            PartyModeManager.player2WinPoints++;

        StartCoroutine(MoveSnail(winner, moveDuration));
        
    }

    IEnumerator MoveSnail(int whichSnail, float duration)
    {
        Transform snail = null;
        Vector3 targetPosition;
        switch (whichSnail)
        {
            case 1:
                snail = snailP1;
                targetPosition = Vector3.Lerp(startP1.position, endP1.position, PartyModeManager.player1WinPoints / (float)PartyModeManager.numToWin);
                HueIsMoving();
                GooIsStill();
                break;
            case 2:
                snail = snailP2;
                targetPosition = Vector3.Lerp(startP2.position, endP2.position, PartyModeManager.player2WinPoints / (float)PartyModeManager.numToWin);
                GooIsMoving();
                HueIsStill();
                break;
            default:
                targetPosition = Vector3.zero;
                break;
        }

        Debug.Log($"Moving {snail}");

        Vector3 startPosition = snail.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Moves along a spherical arc between the two positions
            snail.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GooIsStill();
        HueIsStill();
        PopUpBtn(); 

        // Snap to final position
        transform.position = targetPosition;
    }

    public void GooIsMoving()
    {
        GooController.SetBool("isMoving", true);
    }

    public void GooIsStill()
    {
        GooController.SetBool("isMoving", false);
    }

    public void HueIsMoving()
    {
        HueController.SetBool("isMoving", true);
    }

    public void HueIsStill()
    {
        HueController.SetBool("isMoving", false);
    }

    public void PopUpBtn()
    {
        button.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        button.GetComponent<TitlePulse>().enabled = false;

        float elapsedTime = 0f;
        Vector3 endScale = button.transform.localScale;
        float duration = 1f; 

        while (elapsedTime < duration)
        {
            button.transform.localScale = Vector3.Lerp(Vector3.zero, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.GetComponent<TitlePulse>().enabled = true; 
    }

    public void LoadNext()
    {
        string sceneName = PartyModeManager.toPlay[0];
        PartyModeManager.toPlay.RemoveAt(0);

        foreach (var scene in PartyModeManager.toPlay)
        {
            Debug.Log("Lineup: " + scene);
        }


        SceneManager.LoadScene(sceneName);
    }
}
