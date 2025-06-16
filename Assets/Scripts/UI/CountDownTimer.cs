using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 60f;         // Time in seconds
    private float currentTime;
    public Text timerText;                // Assign in the Inspector
    private bool isRunning = true;
    private bool hasTriggered10Seconds = false;

    [Header("Audio Settings")]
    public AudioClip tickingSound;        // Assign a fast ticking sound
    public AudioClip warningSound;        // Assign an urgent warning sound
    [Range(0, 1)] public float volume = 0.7f;
    private AudioSource audioSource;

    [Header("Screen Shake Settings")]
    public float shakeDuration = 2f;      // How long the shake lasts
    public float shakeMagnitude = 0.2f;   // How intense the shake is
    private Camera mainCamera;
    private Vector3 originalCameraPos;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();

        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        // Get main camera reference
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPos = mainCamera.transform.localPosition;
        }
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;

            // Check for 10-second mark
            if (currentTime <= 10f && !hasTriggered10Seconds)
            {
                hasTriggered10Seconds = true;
                StartCoroutine(PlayTickingSound());
                StartCoroutine(ShakeCamera());
            }

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TimerEnded();
            }
            UpdateTimerDisplay();
        }
    }

    IEnumerator PlayTickingSound()
    {
        // Play warning sound once
        if (warningSound != null)
        {
            audioSource.PlayOneShot(warningSound);
            yield return new WaitForSeconds(warningSound.length);
        }

        // Play ticking sound in a loop until timer ends
        while (currentTime > 0f && isRunning)
        {
            if (tickingSound != null)
            {
                audioSource.PlayOneShot(tickingSound);
                yield return new WaitForSeconds(tickingSound.length);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator ShakeCamera()
    {
        if (mainCamera == null) yield break;

        float elapsed = 0f;

        while (elapsed < shakeDuration && currentTime > 0f)
        {
            // Random offset for shake effect
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalCameraPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset camera position
        mainCamera.transform.localPosition = originalCameraPos;
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        int minutes = seconds / 60;
        seconds = seconds % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";

        // Change color when below 10 seconds
        if (currentTime <= 10f)
        {
            timerText.color = Color.red;
        }
    }

    void TimerEnded()
    {
        Debug.Log("Timer ended!");
        // Stop any ongoing sound effects
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        // Reset camera position
        if (mainCamera != null)
        {
            mainCamera.transform.localPosition = originalCameraPos;
        }
    }

    public void SetTimerPaused(bool pause)
    {
        isRunning = !pause;
        if (pause && audioSource != null)
        {
            audioSource.Pause();
        }
        else if (!pause && audioSource != null && currentTime <= 10f)
        {
            audioSource.UnPause();
        }
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        hasTriggered10Seconds = false;
        timerText.color = Color.white; // Reset text color
        UpdateTimerDisplay();

        // Stop any ongoing sound effects
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        // Reset camera position
        if (mainCamera != null)
        {
            mainCamera.transform.localPosition = originalCameraPos;
        }
    }
}