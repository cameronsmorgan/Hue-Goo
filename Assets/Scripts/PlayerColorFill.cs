using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerColorFill : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap paintTilemap;
    public Tilemap grassTilemap;

    [Header("Paint Tile")]
    public RuleTile paintTile;

    [Header("Player Info")]
    public string playerName = "Player1"; // Set this in the Inspector

    private int totalTargetTiles;
    private int paintedCorrectly;
    private bool hasFinished = false;

    private void Start()
    {
        totalTargetTiles = CountGrassTiles();
        paintedCorrectly = 0;
        hasFinished = false;
    }

    private void Update()
    {
        if (!hasFinished)
        {
            PaintUnderPlayer();
        }
    }

    void PaintUnderPlayer()
    {
        Vector3 worldPos = transform.position;
        Vector3Int cell = paintTilemap.WorldToCell(worldPos);

        if (grassTilemap.HasTile(cell) && !paintTilemap.HasTile(cell))
        {
            paintTilemap.SetTile(cell, paintTile);
            paintedCorrectly++;

            float percentageFilled = (float)paintedCorrectly / totalTargetTiles;

            if (RaceUIManager.Instance != null)
            {
                RaceUIManager.Instance.UpdatePlayerProgress(playerName, percentageFilled);
            }

            Debug.Log($"{playerName} Progress: {paintedCorrectly}/{totalTargetTiles} tiles filled ({percentageFilled * 100f:F1}%)");

            if (paintedCorrectly >= totalTargetTiles)
            {
                hasFinished = true;

                if (Level4GameManager.Instance != null)
                {
                    Level4GameManager.Instance.PlayerFinished(playerName);
                    Debug.Log($"{playerName} has completed their area!");

                    GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Player");

                    foreach (GameObject obj in taggedObjects)
                    {
                        Debug.Log(obj);
                        obj.GetComponent<PlayerMovement>().canMove = false;
                    }
                }
            }
        }
    }

    int CountGrassTiles()
    {
        int count = 0;
        BoundsInt bounds = grassTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (grassTilemap.HasTile(pos))
            {
                count++;
            }
        }

        Debug.Log($"{playerName} has {count} target grass tiles.");
        return count;
    }

    public void ResetForNextRound()
    {
        paintedCorrectly = 0;
        hasFinished = false;
        enabled = true;

        if (paintTilemap != null)
        {
            paintTilemap.ClearAllTiles();
        }

        totalTargetTiles = CountGrassTiles();

        if (RaceUIManager.Instance != null)
        {
            RaceUIManager.Instance.UpdatePlayerProgress(playerName, 0f);
        }
    }
}
