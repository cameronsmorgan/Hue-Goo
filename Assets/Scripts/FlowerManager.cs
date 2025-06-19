using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlowerManager : MonoBehaviour
{
    public GameObject flowerPrefab;
    public Tilemap grassTilemap;
    public Tilemap paintableTilemap;
    public int numberOfFlowers = 10;
    public float revealTime = 5f;
    public float paintTime = 20f;
    public float totalGameTime = 60f;

    private float elapsedGameTime = 0f;

    public int player1Score = 0;
    public int player2Score = 0;

    public PlayerMovement player1;
    public PlayerMovement player2;

    public Text player1ScoreText;
    public Text player2ScoreText;

    [Header("Timer UI")]
    public Text timerText;

    [Header("Audio")]
    public AudioClip flowerCollectSound;
    public AudioClip tickingSound;
    public AudioClip timerEndSound;
    private AudioSource audioSource;
    private AudioSource endAudioSource;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;

    private List<Vector3Int> flowerPositions = new List<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> spawnedFlowers = new Dictionary<Vector3Int, GameObject>();
    private HashSet<Vector3Int> flowersScored = new HashSet<Vector3Int>();

    private bool tickingStarted = false;
    private bool hasPlayedEndSound = false;
    private Vector3 originalCamPos;

    private enum GamePhase { Reveal, Paint, Wait }
    private GamePhase currentPhase = GamePhase.Wait;

    private Color player1Color;
    private Color player2Color;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#78DBD9", out player1Color);
        ColorUtility.TryParseHtmlString("#96DA83", out player2Color);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        endAudioSource = gameObject.AddComponent<AudioSource>();
        endAudioSource.playOnAwake = false;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        originalCamPos = cameraTransform.position;

        StartCoroutine(GameLoop());
        StartCoroutine(UpdateTimerDisplay());
        UpdateScoreUI();
    }

    void Update()
    {
        if (currentPhase != GamePhase.Paint) return;

        foreach (var pos in flowerPositions)
        {
            if (flowersScored.Contains(pos)) continue;

            TileBase paintedTile = paintableTilemap.GetTile(pos);
            if (paintedTile == null) continue;

            string tileName = paintedTile.name.ToLower();

            if (tileName.Contains("hue"))
            {
                player1Score++;
                RevealScoredFlower(pos, player1Color);
                PlayFlowerSound();
            }
            else if (tileName.Contains("goo"))
            {
                player2Score++;
                RevealScoredFlower(pos, player2Color);
                PlayFlowerSound();
            }

            UpdateScoreUI();
        }
    }

    IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(0.25f);
        while (elapsedGameTime <= totalGameTime)
        {
            paintableTilemap.ClearAllTiles();
            SpawnFlowers();

            currentPhase = GamePhase.Reveal;
            SetPlayerMovement(false);
            ShowFlowers(true);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(revealTime);

            Time.timeScale = 1f;
            currentPhase = GamePhase.Paint;
            ShowFlowers(false);
            SetPlayerMovement(true);
            yield return new WaitForSeconds(paintTime);

            CheckPaintedFlowers();
            CleanupFlowers();

            elapsedGameTime += revealTime + paintTime;
        }

        EndGame();
    }

    IEnumerator UpdateTimerDisplay()
    {
        float timeLeft = totalGameTime;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timerText != null)
            {
                int seconds = Mathf.CeilToInt(timeLeft);
                timerText.text = seconds.ToString();

                // Change color when < 10 seconds
                if (seconds <= 10)
                {
                    timerText.color = Color.red;

                    if (!tickingStarted && tickingSound != null)
                    {
                        tickingStarted = true;
                        audioSource.loop = true;
                        audioSource.clip = tickingSound;
                        audioSource.Play();
                    }

                    StartCoroutine(ShakeCamera());
                }

                if (seconds == 1 && !hasPlayedEndSound)
                {
                    hasPlayedEndSound = true;
                    if (timerEndSound != null)
                        endAudioSource.PlayOneShot(timerEndSound);
                }
            }

            yield return null;
        }

        if (audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        timerText.text = "0";
    }

    IEnumerator ShakeCamera()
    {
        Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
        randomOffset.z = 0f;

        cameraTransform.position = originalCamPos + randomOffset;
        yield return new WaitForSeconds(shakeDuration);
        cameraTransform.position = originalCamPos;
    }

    void SetPlayerMovement(bool canMove)
    {
        if (player1 != null) player1.canMove = canMove;
        if (player2 != null) player2.canMove = canMove;
    }

    void SpawnFlowers()
    {
        flowerPositions.Clear();
        spawnedFlowers.Clear();

        BoundsInt bounds = grassTilemap.cellBounds;

        int placed = 0;
        while (placed < numberOfFlowers)
        {
            int x = Random.Range(bounds.xMin, bounds.xMax);
            int y = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int pos = new Vector3Int(x, y, 0);

            if (!grassTilemap.HasTile(pos) || flowerPositions.Contains(pos))
                continue;

            GameObject flower = Instantiate(flowerPrefab, grassTilemap.GetCellCenterWorld(pos), Quaternion.identity);
            flower.transform.SetParent(this.transform);
            flowerPositions.Add(pos);
            spawnedFlowers[pos] = flower;

            placed++;
        }
    }

    void ShowFlowers(bool visible)
    {
        foreach (var flower in spawnedFlowers.Values)
        {
            flower.GetComponent<SpriteRenderer>().enabled = visible;
        }
    }

    void CheckPaintedFlowers()
    {
        foreach (var pos in flowerPositions)
        {
            TileBase paintedTile = paintableTilemap.GetTile(pos);
            if (paintedTile == null) continue;

            string tileName = paintedTile.name.ToLower();
            if (tileName.Contains("red")) player1Score++;
            else if (tileName.Contains("blue")) player2Score++;

            GameObject flower = spawnedFlowers[pos];
            SpriteRenderer sr = flower.GetComponent<SpriteRenderer>();
            sr.enabled = true;
            sr.color = tileName.Contains("red") ? Color.red : Color.blue;
            sr.sortingOrder = 10;

            Vector3 offset = new Vector3(0, 0, -0.1f);
            flower.transform.position = grassTilemap.GetCellCenterWorld(pos) + offset;

            PlayFlowerSound();
            UpdateScoreUI();
        }
    }

    void CleanupFlowers()
    {
        foreach (var flower in spawnedFlowers.Values)
        {
            Destroy(flower);
        }

        flowerPositions.Clear();
        spawnedFlowers.Clear();
        flowersScored.Clear();
    }

    void EndGame()
    {
        if (player1Score > player2Score)
        {
            PartyModeManager.lastRoundWinner = 1;
            SceneManager.LoadScene("UltimateWinner");
        }
        else if (player2Score > player1Score)
        {
            PartyModeManager.lastRoundWinner = 2;
            SceneManager.LoadScene("UltimateWinner");
        }
        else
        {
            PartyModeManager.lastRoundWinner = 3;
            SceneManager.LoadScene("UltimateWinner");
        }
    }

    void RevealScoredFlower(Vector3Int pos, Color color)
    {
        if (spawnedFlowers.TryGetValue(pos, out GameObject flower))
        {
            SpriteRenderer sr = flower.GetComponent<SpriteRenderer>();
            sr.color = color;
            sr.enabled = true;
        }

        flowersScored.Add(pos);
    }

    void PlayFlowerSound()
    {
        if (flowerCollectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(flowerCollectSound);
        }
    }

    void UpdateScoreUI()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = "Player 1: " + player1Score;
        if (player2ScoreText != null)
            player2ScoreText.text = "Player 2: " + player2Score;
    }
}
