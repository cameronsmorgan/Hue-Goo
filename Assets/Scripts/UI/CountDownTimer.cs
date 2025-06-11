using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f;         // Time in seconds
    private float currentTime;

    public Text timerText;                // Assign in the Inspector
    private bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TimerEnded();
            }
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        int minutes = seconds / 60;
        seconds = seconds % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimerEnded()
    {
        Debug.Log("Timer ended!");
        // You can trigger game over, load a scene, etc.
    }

    // Optional: Call this to pause or resume the timer externally
    public void SetTimerPaused(bool pause)
    {
        isRunning = !pause;
    }

    // Optional: Reset timer to original value
    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        UpdateTimerDisplay();
    }
}
