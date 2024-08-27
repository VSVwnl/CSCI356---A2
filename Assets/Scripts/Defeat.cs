using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class Defeat : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    void Update()
    {
        if (playerHealth.health <= 0)
        {
            LoadDefeatScene();
        }
    }

    void LoadDefeatScene()
    {
        SceneManager.LoadScene("Defeat");
    }
}
