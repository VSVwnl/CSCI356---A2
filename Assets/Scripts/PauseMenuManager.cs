using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this to use UI elements

public class PauseMenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public MouseLook mouseLook;
    public GameObject pauseMenuUI; // Reference to your pause menu UI
    public Slider sensitivitySlider; // Reference to your sensitivity slider

    private bool isPaused = false;

    private void Start()
    {
        // Load and apply saved settings
        LoadSettings();

        // Initialize the sensitivity slider value based on MouseLook settings
        if (sensitivitySlider != null && mouseLook != null)
        {
            sensitivitySlider.value = mouseLook.sensitivityHor;
        }
    }

    private void Update()
    {
        // Toggle pause menu with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void setVolume(float volume)
    {
        Debug.Log("Volume = " + volume);
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void setMouseSensitivity(float sensitivity)
    {
        Debug.Log("Sens = " + sensitivity);
        mouseLook.sensitivityHor = sensitivity;
        mouseLook.sensitivityVert = sensitivity;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);

        // Update slider value to reflect the current sensitivity
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sensitivity;
        }
    }

    public void setDisplayMode(int mode)
    {
        Debug.Log("Display mode selected: " + mode);

        switch (mode)
        {
            case 0: // FullScreen
                Debug.Log("Display mode selected: FullScreen");
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Borderless Windowed
                Debug.Log("Display mode selected: WindowedFullScreen");
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Windowed
                Debug.Log("Display mode selected: Windowed");
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Debug.LogWarning("Invalid display mode selected.");
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", mode);
    }

    private void LoadSettings()
    {
        // Load volume setting
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            setVolume(savedVolume);
        }

        // Load mouse sensitivity setting
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity");
            setMouseSensitivity(savedSensitivity);
        }

        // Load display mode setting
        if (PlayerPrefs.HasKey("DisplayMode"))
        {
            int savedMode = PlayerPrefs.GetInt("DisplayMode");
            setDisplayMode(savedMode);
        }
    }

    public void ReturnGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Pause game time
        isPaused = true;

        // Update the slider value to the current sensitivity when the pause menu is opened
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", mouseLook.sensitivityHor);
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume game time
        isPaused = false;

        // Optionally, you could reapply any settings or updates here if needed
    }
}
