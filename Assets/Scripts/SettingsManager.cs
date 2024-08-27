using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mainMixer; // Main game audio mixer
    public CameraLook cameraLook;
    public Slider sensitivitySlider;
    public Slider volumeSlider; // Volume slider for controlling multiple mixers

    private void Start()
    {
        // Load and apply saved settings
        LoadSettings();

        // Initialize sensitivity slider with saved sensitivity
        if (sensitivitySlider != null && PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }

        // Add listener to update sensitivity when slider changes
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(delegate { setMouseSensitivity(sensitivitySlider.value); });
        }

        // Initialize volume slider with saved volume
        if (volumeSlider != null && PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        // Add listener to update volume when slider changes
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { setVolume(volumeSlider.value); });
        }
    }

    public void setVolume(float volume)
    {
        Debug.Log("Volume = " + volume);
        // Convert volume to decibels
        float volumeInDb = Mathf.Log10(volume) * 20;

        // Set the volume for the main mixer
        if (mainMixer != null)
        {
            mainMixer.SetFloat("Volume", volumeInDb);
        }

        PlayerPrefs.SetFloat("Volume", volume); // Save the volume setting
        PlayerPrefs.Save(); // Save all changes immediately
    }

    public void setMouseSensitivity(float sensitivity)
    {
        Debug.Log("Sensitivity = " + sensitivity);
        cameraLook.sensitivity = new Vector2(sensitivity, sensitivity); // Adjust both X and Y sensitivity
        PlayerPrefs.SetFloat("Sensitivity", sensitivity); // Save the sensitivity setting
        PlayerPrefs.Save(); // Save all changes immediately
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

        PlayerPrefs.SetInt("DisplayMode", mode); // Save the display mode setting
        PlayerPrefs.Save(); // Save all changes immediately
    }

    private void LoadSettings()
    {
        // Load volume setting
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            setVolume(savedVolume); // Apply saved volume
        }

        // Load mouse sensitivity setting
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity");
            setMouseSensitivity(savedSensitivity); // Apply saved sensitivity
        }

        // Load display mode setting
        if (PlayerPrefs.HasKey("DisplayMode"))
        {
            int savedMode = PlayerPrefs.GetInt("DisplayMode");
            setDisplayMode(savedMode); // Apply saved display mode
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
