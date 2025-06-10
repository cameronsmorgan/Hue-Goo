using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerritoryTracker : MonoBehaviour
{
    public Tilemap paintTilemap;
    public TileBase redTile;
    public TileBase blueTile;
    public Tilemap walkableAreaTilemap; // Reference to your playable area tilemap

    private int redTiles = 0;
    private int blueTiles = 0;
    private int totalPaintableTiles = 0;

    private TerritoryUI uiManager;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<TerritoryUI>();
    }

    private void Start()
    {
        CalculatePaintableArea();
        InvokeRepeating("UpdateTerritory", 0.5f, 0.5f); // More frequent updates
    }

    private void CalculatePaintableArea()
    {
        totalPaintableTiles = 0;
        paintTilemap.CompressBounds();
        var bounds = walkableAreaTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (walkableAreaTilemap.HasTile(pos))
                totalPaintableTiles++;
        }

        Debug.Log($"Total paintable tiles: {totalPaintableTiles}");
    }

    public void UpdateTerritory()
    {
        redTiles = 0;
        blueTiles = 0;
        paintTilemap.CompressBounds();
        var bounds = paintTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            var tile = paintTilemap.GetTile(pos);
            if (tile == redTile) redTiles++;
            else if (tile == blueTile) blueTiles++;
        }

        int totalPainted = redTiles + blueTiles;
        float redPercent = totalPaintableTiles > 0 ?
            Mathf.Round((float)redTiles / totalPaintableTiles * 100) : 0;
        float bluePercent = totalPaintableTiles > 0 ?
            Mathf.Round((float)blueTiles / totalPaintableTiles * 100) : 0;

        Debug.Log($"Red: {redTiles} ({redPercent}%) | Blue: {blueTiles} ({bluePercent}%) | Total: {totalPainted}/{totalPaintableTiles}");

        if (uiManager != null)
            uiManager.UpdateUI(redPercent, bluePercent);
    }

    // Visualization for debugging
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        paintTilemap.CompressBounds();
        var bounds = paintTilemap.cellBounds;

        Gizmos.color = Color.red;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (paintTilemap.GetTile(pos) == redTile)
                Gizmos.DrawCube(paintTilemap.GetCellCenterWorld(pos), Vector3.one * 0.4f);
        }

        Gizmos.color = Color.blue;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (paintTilemap.GetTile(pos) == blueTile)
                Gizmos.DrawCube(paintTilemap.GetCellCenterWorld(pos), Vector3.one * 0.4f);
        }
    }
}
