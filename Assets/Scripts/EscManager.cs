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
        EnableGameControls(true); // Re-enable controls
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
        EnableGameControls(false); // Disable controls
    }

    public void OpenSettings()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading settings
        // Store the current scene name before loading settings
        GameManager.currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading main menu
        SceneManager.LoadScene("MainMenu");
    }

    private void EnableGameControls(bool enable)
    {
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
