using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 900; // 15 minutes in seconds
    public Text timerText;
    public Text bossWarningText; // New text to show when the timer hits 0

    void Start()
    {
        bossWarningText.gameObject.SetActive(false); // Hide boss warning text at the start
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            TriggerBoss();
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Change color to red if time is less than 5 minutes (300 seconds)
        if (timeToDisplay <= 300)
        {
            timerText.color = Color.red;
        }

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TriggerBoss()
    {
        // Show boss warning text
        bossWarningText.gameObject.SetActive(true);
        bossWarningText.text = "The Boss is here!";

        // Logic to trigger boss appearance
        Debug.Log("Boss is coming!");
    }
}
