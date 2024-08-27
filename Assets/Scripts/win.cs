using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class win : MonoBehaviour
{
    public ObjectCollector objectCollector; // Reference to the ObjectCollector script

    void Update()
    {
        // Check if the total collected items meets or exceeds the required amount
        if (objectCollector.totalCollected >= objectCollector.requiredCollectibles)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Load the next scene specified in the inspector
        SceneManager.LoadScene("Victory");
    }
}

