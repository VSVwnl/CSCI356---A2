using UnityEngine;
using UnityEngine.SceneManagement;

public class EscManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        EnableGameControls(true); // Example function to re-enable controls
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
        // Optionally, disable or pause certain game components here
        EnableGameControls(false); // Example function to disable controls
    }

    public void OpenSettings()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading settings
        SceneManager.LoadScene("PauseMenu");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading main menu
        SceneManager.LoadScene("MainMenu");
    }

    private void EnableGameControls(bool enable)
    {
        // Example function to enable or disable game controls
        // Replace with your actual control scripts or methods
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = enable;
            }
        }
    }
}
