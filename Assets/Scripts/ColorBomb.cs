using UnityEngine;
using UnityEngine.Tilemaps;
public class ColorBomb : MonoBehaviour
{
    public float radius = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PaintTrail paintTrail = collision.GetComponent<PaintTrail>();

        if (paintTrail != null)
        {
            ExplodePaint(paintTrail);
            Destroy(gameObject);
        }
    }

    void ExplodePaint(PaintTrail paintTrail)
    {
        Tilemap tilemap = paintTrail.paintTilemap;
        TileBase targetTile = paintTrail.isRed ? paintTrail.redPaintTile : paintTrail.bluePaintTile;
        Vector3Int centerCell = tilemap.WorldToCell(transform.position);

        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.FloorToInt(radius); y++)
            {
                Vector3Int cell = new Vector3Int(centerCell.x + x, centerCell.y + y, 0);
                if (Vector3Int.Distance(centerCell, cell) <= radius)
                {
                    tilemap.SetTile(cell, targetTile);
                }
            }
        }
    }
}
