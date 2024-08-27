using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public Button fullScreenButton;
    public Button windowedFullScreenButton;
    public Button windowedButton;

    private void Start()
    {
        LoadSettings();

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }

        if (fullScreenButton != null)
        {
            fullScreenButton.onClick.AddListener(() => SetDisplayMode(0)); // FullScreen
        }
        if (windowedFullScreenButton != null)
        {
            windowedFullScreenButton.onClick.AddListener(() => SetDisplayMode(1)); // Windowed FullScreen
        }
        if (windowedButton != null)
        {
            windowedButton.onClick.AddListener(() => SetDisplayMode(2)); // Windowed
        }
    }

    public void SetVolume()
    {
        if (volumeSlider != null)
        {
            Debug.Log("Volume = " + volumeSlider.value);
            AudioListener.volume = volumeSlider.value;

            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
            PlayerPrefs.Save();
        }
    }

    public void SetDisplayMode(int mode)
    {
        Debug.Log("Display mode selected: " + mode);

        switch (mode)
        {
            case 0: // FullScreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Borderless Windowed
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Debug.LogWarning("Invalid display mode selected.");
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", mode);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = savedVolume;
            SetVolume();
        }
        else
        {
            volumeSlider.value = 1.0f;
            SetVolume();
        }

        if (PlayerPrefs.HasKey("DisplayMode"))
        {
            int savedMode = PlayerPrefs.GetInt("DisplayMode");
            SetDisplayMode(savedMode);
        }
    }

    public void ReturnToPreviousScene()
    {
        // Unload the settings scene only
        SceneManager.UnloadSceneAsync("Settings");
    }
}
