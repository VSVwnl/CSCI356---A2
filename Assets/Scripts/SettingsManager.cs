using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
        public AudioMixer audioMixer;
        public MouseLook mouseLook;

        private void Start()
        {
            // Load and apply saved settings
            LoadSettings();
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

        public void ReturnMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
}
