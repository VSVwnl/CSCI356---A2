using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    void Start()
    {
        // Ensure the cursor is visible and unlocked in the victory scene
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
