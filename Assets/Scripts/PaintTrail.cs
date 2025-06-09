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
        TileBase currentTile = paintTilemap.GetTile(tilePosition);
        TileBase targetTile = isRed ? redPaintTile : bluePaintTile;

        if (currentTile != targetTile)
        {
            paintTilemap.SetTile(tilePosition, targetTile);
        }
    }
}
