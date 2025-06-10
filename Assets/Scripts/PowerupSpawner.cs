using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
public class PowerupSpawner : MonoBehaviour
{

    public GameObject colourBombPrefab;
    public Tilemap tilemap; // Used for position alignment
    public TilemapCollider2D borderTilemapCollider; // Border collider to avoid
    public Tilemap walkableTilemap; // Tilemap where powerups can spawn

    public float spawnInterval = 10f;
    public int maxSpawnAttempts = 20; // Prevent infinite loops

    private void Start()
    {
        InvokeRepeating(nameof(SpawnColorBomb), spawnInterval, spawnInterval);
    }

    private void SpawnColorBomb()
    {
        Vector2? validPosition = GetValidSpawnPosition();
        if (validPosition.HasValue)
        {
            Instantiate(colourBombPrefab, validPosition.Value, Quaternion.identity);
        }
    }

    private Vector2? GetValidSpawnPosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Get random position within walkable area
            Vector3Int randomCell = GetRandomWalkableCell();
            Vector2 spawnPosition = tilemap.GetCellCenterWorld(randomCell);

            // Verify the position isn't on the border
            if (!IsPositionOnBorder(spawnPosition))
            {
                return spawnPosition;
            }
        }
        return null;
    }

    private Vector3Int GetRandomWalkableCell()
    {
        BoundsInt bounds = walkableTilemap.cellBounds;
        return new Vector3Int(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax),
            0
        );
    }

    private bool IsPositionOnBorder(Vector2 position)
    {
        // Check if position overlaps with border collider
        return borderTilemapCollider.OverlapPoint(position);
    }

    /* Title: Random Position Within a 2d Collider
     * Author: PapayaLimon
     * Date: 13 March 2025
     * Code version: Unity 2022.2
     * Availability: https://discussions.unity.com/t/random-position-inside-2d-collider/907682
     * (Modified to work with TilemapCollider2D)
     */
}
