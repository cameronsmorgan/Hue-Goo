using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlowerManager : MonoBehaviour
{
    public GameObject flowerPrefab;
    public Tilemap grassTilemap;
    public Tilemap paintableTilemap;
    public int numberOfFlowers = 10;
    public float revealTime = 5f;
    public float paintTime = 20f;
    public float totalGameTime = 60f;

    private List<Vector3Int> flowerPositions = new List<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> spawnedFlowers = new Dictionary<Vector3Int, GameObject>();

    private float elapsedGameTime = 0f;
    private float roundTimer = 0f;

    public int player1Score = 0;
    public int player2Score = 0;

    public PlayerMovement player1;
    public PlayerMovement player2;

    private enum GamePhase { Reveal, Paint, Wait }
    private GamePhase currentPhase = GamePhase.Wait;
    private HashSet<Vector3Int> flowersScored = new HashSet<Vector3Int>();

    void Start()
    {
        StartCoroutine(GameLoop());
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

        if (tileName.Contains("red"))
        {
            player1Score++;
            RevealScoredFlower(pos, Color.red);
        }
        else if (tileName.Contains("blue"))
        {
            player2Score++;
            RevealScoredFlower(pos, Color.blue);
        }
    }
    }


   IEnumerator GameLoop()
   {
    while (elapsedGameTime < totalGameTime)
    {
        SpawnFlowers();

        currentPhase = GamePhase.Reveal;
        SetPlayerMovement(false); // 🚫 DISABLE movement
        ShowFlowers(true);
        yield return new WaitForSeconds(revealTime);

        currentPhase = GamePhase.Paint;
        ShowFlowers(false);
        SetPlayerMovement(true); // ✅ ENABLE movement
        yield return new WaitForSeconds(paintTime);

        CheckPaintedFlowers();
        CleanupFlowers();

        elapsedGameTime += revealTime + paintTime;
    }

    EndGame();
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
            if (tileName.Contains("red"))
            {
                player1Score++;
            }
            else if (tileName.Contains("blue"))
            {
                player2Score++;
            }

            GameObject flower = spawnedFlowers[pos];
SpriteRenderer sr = flower.GetComponent<SpriteRenderer>();

// Make it visible and change color
sr.enabled = true;
sr.color = tileName.Contains("red") ? Color.red : Color.blue;

// Ensure it renders above the slime
sr.sortingOrder = 10; // Higher number = renders on top

// Optional: Slight Z-offset to appear above ground visually
Vector3 offset = new Vector3(0, 0, -0.1f);
flower.transform.position = grassTilemap.GetCellCenterWorld(pos) + offset;

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
        Debug.Log($"Game Over! Player 1: {player1Score}, Player 2: {player2Score}");
        if (player1Score > player2Score)
            Debug.Log("Player 1 Wins!");
        else if (player2Score > player1Score)
            Debug.Log("Player 2 Wins!");
        else
            Debug.Log("It's a Tie!");
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

}