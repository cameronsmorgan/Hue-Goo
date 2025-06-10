using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintTrail : MonoBehaviour
{
    public Tilemap paintTilemap;          
    public TileBase redPaintTile;         
    public TileBase bluePaintTile;    
    public bool isRed = true;        

    void Update()
    {
        Vector3Int tilePosition = paintTilemap.WorldToCell(transform.position);

        // Only set tile if we're on a new position
        if (!paintTilemap.HasTile(tilePosition) ||
            paintTilemap.GetTile(tilePosition) != (isRed ? redPaintTile : bluePaintTile))
        {
            paintTilemap.SetTile(tilePosition, isRed ? redPaintTile : bluePaintTile);
        }

        CheckForOverwrite(tilePosition);

    }

    private void CheckForOverwrite(Vector3Int cellPosition)
    {
        TileBase currentTile = paintTilemap.GetTile(cellPosition);

        if (isRed && currentTile == bluePaintTile)
        {
            // Player 1 is covering Player 2's territory
            TerritoryTracker tracker = FindFirstObjectByType<TerritoryTracker>();
            if (tracker != null) tracker.UpdateTerritory();
        }
        else if (!isRed && currentTile == redPaintTile)
        {
            // Player 2 is covering Player 1's territory
            TerritoryTracker tracker = FindFirstObjectByType<TerritoryTracker>();
            if (tracker != null) tracker.UpdateTerritory();
        }
    }

}
