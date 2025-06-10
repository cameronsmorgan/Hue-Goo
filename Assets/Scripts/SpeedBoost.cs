using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostDuration = 3f;
    public float boostMultiplier = 0.5f;
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
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            // Play collection sound
            if (collectionSound != null)
            {
                audioSource.PlayOneShot(collectionSound, volume);
            }

            player.ApplySpeedBoost(boostMultiplier, boostDuration);

            // Disable visuals/collider immediately but delay destruction
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Destroy after sound finishes playing
            Destroy(gameObject, collectionSound != null ? collectionSound.length : 0.1f);
        }
    }
}
