using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 60f;
    private float currentTime;
    public Text timerText;
    private bool isRunning = true;
    private bool hasTriggered10Seconds = false;
    private bool hasPlayedEndSound = false;

    [Header("Audio Settings")]
    public AudioClip tickingSound;
    public AudioClip warningSound;
    public AudioClip timerEndSound;
    [Range(0, 1)] public float volume = 0.7f;

    private AudioSource tickingAudioSource;
    private AudioSource endAudioSource;

    [Header("Screen Shake Settings")]
    public float shakeDuration = 2f;
    public float shakeMagnitude = 0.2f;
    private Camera mainCamera;
    private Vector3 originalCameraPos;

    // Timer end event for external listeners
    public delegate void TimerEndAction();
    public event TimerEndAction OnTimerEnd;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();

        // Audio sources
        tickingAudioSource = gameObject.AddComponent<AudioSource>();
        tickingAudioSource.playOnAwake = false;
        tickingAudioSource.volume = volume;

        endAudioSource = gameObject.AddComponent<AudioSource>();
        endAudioSource.playOnAwake = false;
        endAudioSource.volume = volume;

        // Camera
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalCameraPos = mainCamera.transform.localPosition;
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 10f && !hasTriggered10Seconds)
            {
                hasTriggered10Seconds = true;
                StartCoroutine(PlayTickingSound());
                StartCoroutine(ShakeCamera());
            }

            if (currentTime <= 1f && !hasPlayedEndSound)
            {
                hasPlayedEndSound = true;
                if (timerEndSound != null && endAudioSource != null)
                {
                    endAudioSource.PlayOneShot(timerEndSound);
                }
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

    void TimerEnded()
    {
        Debug.Log("Timer ended!");

        if (tickingAudioSource != null)
            tickingAudioSource.Stop();

        if (mainCamera != null)
            mainCamera.transform.localPosition = originalCameraPos;

        OnTimerEnd?.Invoke();
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        int minutes = seconds / 60;
        seconds = seconds % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (currentTime <= 10f)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    IEnumerator PlayTickingSound()
    {
        if (warningSound != null)
        {
            tickingAudioSource.PlayOneShot(warningSound);
            yield return new WaitForSeconds(warningSound.length);
        }

        while (currentTime > 0f && isRunning)
        {
            if (tickingSound != null)
            {
                tickingAudioSource.PlayOneShot(tickingSound);
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
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalCameraPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPos;
    }

    public void SetTimerPaused(bool pause)
    {
        isRunning = !pause;
        if (pause && tickingAudioSource != null) tickingAudioSource.Pause();
        else if (!pause && tickingAudioSource != null && currentTime <= 10f) tickingAudioSource.UnPause();
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        hasTriggered10Seconds = false;
        hasPlayedEndSound = false;
        timerText.color = Color.white;
        UpdateTimerDisplay();

        if (tickingAudioSource != null) tickingAudioSource.Stop();
        if (mainCamera != null) mainCamera.transform.localPosition = originalCameraPos;
    }
}

