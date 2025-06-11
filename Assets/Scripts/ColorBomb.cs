using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorBomb : MonoBehaviour
{
    public float radius = 2f;
    public AudioClip collectionSound;
    [Range(0, 1)] public float volume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PaintTrail paintTrail = collision.GetComponent<PaintTrail>();

        if (paintTrail != null)
        {
            // Play collection sound
            if (collectionSound != null)
            {
                audioSource.PlayOneShot(collectionSound, volume);
            }

            ExplodePaint(paintTrail);

            // Disable visuals/collider immediately but delay destruction
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Destroy after sound finishes playing
            Destroy(gameObject, collectionSound != null ? collectionSound.length : 0.1f);
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
