using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostDuration = 3f;
    public float boostMultiplier = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.ApplySpeedBoost(boostMultiplier, boostDuration);
            Destroy(gameObject);
        }
    }
}
