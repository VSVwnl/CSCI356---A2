using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class Defeat : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    public Timer timerScript; // Reference to the Timer script

    void Update()
    {
        if (playerHealth.health <= 0 || timerScript.timeRemaining <= 0)
        {
            LoadDefeatScene();
        }
    }

    void LoadDefeatScene()
    {
        SceneManager.LoadScene("Defeat");
    }
}
