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
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("PauseMenu");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading main menu
        SceneManager.LoadScene("MainMenu");
    }
}
