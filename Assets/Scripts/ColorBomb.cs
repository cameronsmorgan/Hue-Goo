using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorBomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 2f;

    [Header("Audio Settings")]
    public AudioClip collectionSound;
    [Range(0, 1)] public float volume = 1f;

    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    private AudioSource audioSource;
    private Camera mainCamera;

    private void Awake()
    {
        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        // Get main camera reference
        mainCamera = Camera.main;
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

            // Trigger camera shake
            if (mainCamera != null)
            {
                StartCoroutine(ShakeCamera());
            }

            ExplodePaint(paintTrail);

            // Disable visuals/collider immediately
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Destroy after sound finishes playing
            Destroy(gameObject, collectionSound != null ? collectionSound.length : 0.1f);
        }
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPosition;
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