using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public AudioMixer mainMixer; // Main game audio mixer
    public Slider brightnessSlider;
    public Slider volumeSlider; // Volume slider for controlling multiple mixers

    private void Start()
    {
        // Load and apply saved settings
        LoadSettings();

        // Initialize brightness slider with saved brightness
        if (brightnessSlider != null && PlayerPrefs.HasKey("Brightness"))
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
        }

        // Add listener to update brightness when slider changes
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(delegate { SetBrightness(brightnessSlider.value); });
        }

        // Initialize volume slider with saved volume
        if (volumeSlider != null && PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        // Add listener to update volume when slider changes
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
        }
    }

    public void SetVolume(float volume)
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

    public void SetBrightness(float brightness)
    {
        Debug.Log("Setting brightness to: " + brightness);
        // Update brightness setting here
        // This example assumes brightness affects a material or UI element.
        // Implement your own logic to apply brightness changes in the game.
        RenderSettings.ambientIntensity = brightness; // Example of setting ambient light intensity

        PlayerPrefs.SetFloat("Brightness", brightness); // Save the brightness setting
        PlayerPrefs.Save(); // Save all changes immediately
    }

    public void SetDisplayMode(int mode)
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
            SetVolume(savedVolume); // Apply saved volume
        }

        // Load brightness setting
        if (PlayerPrefs.HasKey("Brightness"))
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness");
            SetBrightness(savedBrightness); // Apply saved brightness
        }

        // Load display mode setting
        if (PlayerPrefs.HasKey("DisplayMode"))
        {
            int savedMode = PlayerPrefs.GetInt("DisplayMode");
            SetDisplayMode(savedMode); // Apply saved display mode
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Game");
    }
}
