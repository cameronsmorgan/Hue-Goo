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

    [Header("Audio Settings")]
    public AudioClip tickingSound;
    public AudioClip warningSound;
    [Range(0, 1)] public float volume = 0.7f;
    private AudioSource audioSource;

    [Header("Screen Shake Settings")]
    public float shakeDuration = 2f;
    public float shakeMagnitude = 0.2f;
    private Camera mainCamera;
    private Vector3 originalCameraPos;

    // ✅ Timer end event for external listeners (like GameManager)
    public delegate void TimerEndAction();
    public event TimerEndAction OnTimerEnd;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

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

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TimerEnded();
            }

            UpdateTimerDisplay();
        }
    }

    // ✅ Called when timer hits 0
    void TimerEnded()
    {
        Debug.Log("Timer ended!");

        if (audioSource != null)
            audioSource.Stop();

        if (mainCamera != null)
            mainCamera.transform.localPosition = originalCameraPos;

        // ✅ Notify other scripts
        OnTimerEnd?.Invoke();
    }

    // ✅ Update the text display
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

    // ✅ Play ticking and warning sounds
    IEnumerator PlayTickingSound()
    {
        if (warningSound != null)
        {
            audioSource.PlayOneShot(warningSound);
            yield return new WaitForSeconds(warningSound.length);
        }

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

    // ✅ Shake the camera for visual feedback
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

    // ✅ Optional pause/resume/reset functionality
    public void SetTimerPaused(bool pause)
    {
        isRunning = !pause;
        if (pause && audioSource != null) audioSource.Pause();
        else if (!pause && audioSource != null && currentTime <= 10f) audioSource.UnPause();
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        hasTriggered10Seconds = false;
        timerText.color = Color.white;
        UpdateTimerDisplay();

        if (audioSource != null) audioSource.Stop();
        if (mainCamera != null) mainCamera.transform.localPosition = originalCameraPos;
    }
}
