using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public int startHour = 6;
    public int minutesPerDay = 24 * 60; // 24 hours * 60 minutes
    public TMP_Text timerText;

    // Reference to your Controller script
    public Controller controller;

    private float currentTime;

    public void startClock()
    {
        currentTime = (float)startHour * 60f;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        
            currentTime += Time.deltaTime;

            if (currentTime >= minutesPerDay)
            {
                // A new day starts
                currentTime = 0f;
            }

            UpdateTimerDisplay();
    }
    

    void UpdateTimerDisplay()
    {
        // Calculate hours and minutes
        int hours = Mathf.FloorToInt(currentTime / 60f);
        int minutes = Mathf.FloorToInt(currentTime % 60f);

        // Display the time in 12-hour format (AM/PM)
        string amPm = (hours < 12) ? "AM" : "PM";
        hours = (hours % 12 == 0) ? 12 : hours % 12;

        timerText.text = string.Format("{0:00}:{1:00} {2}", hours, minutes, amPm);
    }
}
