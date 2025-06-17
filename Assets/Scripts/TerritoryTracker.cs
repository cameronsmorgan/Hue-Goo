using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TerritoryTracker : MonoBehaviour
{
    public Tilemap paintTilemap;
    public TileBase redTile;
    public TileBase blueTile;
    public Tilemap walkableAreaTilemap;

    private int redTiles = 0;
    private int blueTiles = 0;
    private int totalPaintableTiles = 0;

    private TerritoryUI uiManager;
    private CountdownTimer countdownTimer;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<TerritoryUI>();
        countdownTimer = FindFirstObjectByType<CountdownTimer>();

        if (countdownTimer != null)
        {
            countdownTimer.OnTimerEnd += OnTimerFinished;
        }
    }

    private void Start()
    {
        CalculatePaintableArea();
        InvokeRepeating("UpdateTerritory", 0.5f, 0.5f);
    }

    private void CalculatePaintableArea()
    {
        totalPaintableTiles = 0;
        var bounds = walkableAreaTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (walkableAreaTilemap.HasTile(pos))
                totalPaintableTiles++;
        }
    }

    public void UpdateTerritory()
    {
        redTiles = 0;
        blueTiles = 0;
        var bounds = paintTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            var tile = paintTilemap.GetTile(pos);
            if (tile == redTile) redTiles++;
            else if (tile == blueTile) blueTiles++;
        }

        float redPercent = totalPaintableTiles > 0 ? Mathf.Round((float)redTiles / totalPaintableTiles * 100) : 0;
        float bluePercent = totalPaintableTiles > 0 ? Mathf.Round((float)blueTiles / totalPaintableTiles * 100) : 0;

        if (uiManager != null)
            uiManager.UpdateUI(redPercent, bluePercent);
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer ended. Checking winner...");

        if (redTiles > blueTiles)
        {
            PartyModeManager.lastRoundWinner = 2;
            SceneManager.LoadScene("UltimateWinner");
        }
        else if (blueTiles > redTiles)
        {
            PartyModeManager.lastRoundWinner = 1;
            SceneManager.LoadScene("UltimateWinner");
        }
        else
        {
            PartyModeManager.lastRoundWinner = 3;
            SceneManager.LoadScene("UltimateWinner");
        }
    }
}
