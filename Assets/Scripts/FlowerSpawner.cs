using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class FlowerSpawner : MonoBehaviour
{
    public GameObject flowerPrefab;
    public Tilemap tilemap; // Used for converting cell to world
    public TilemapCollider2D borderTilemapCollider;
    public Tilemap walkableTilemap; // Where flowers are allowed to spawn
    public Transform flowerParent; // Parent object for organizing (e.g., powerupTilemap)

    public float spawnInterval = 10f;
    public int numberOfFlowers = 5;
    public int maxSpawnAttempts = 50;

    private void Start()
    {
        SpawnInitialFlowers();
        InvokeRepeating(nameof(SpawnSingleFlower), spawnInterval, spawnInterval);
    }

    private void SpawnInitialFlowers()
    {
        for (int i = 0; i < numberOfFlowers; i++)
        {
            SpawnSingleFlower();
        }
    }

    private void SpawnSingleFlower()
    {
        Vector2? validPosition = GetValidSpawnPosition();
        if (validPosition.HasValue)
        {
            GameObject flower = Instantiate(flowerPrefab, validPosition.Value, Quaternion.identity);

            if (flowerParent != null)
                flower.transform.parent = flowerParent;
        }
    }

    private Vector2? GetValidSpawnPosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3Int cell = GetRandomWalkableCell();

            if (!walkableTilemap.HasTile(cell))
                continue;

            Vector2 worldPosition = tilemap.GetCellCenterWorld(cell);

            if (!IsPositionOnBorder(worldPosition))
                return worldPosition;
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
        return borderTilemapCollider != null && borderTilemapCollider.OverlapPoint(position);
    }
}
