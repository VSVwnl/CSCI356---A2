using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisablePrefab : MonoBehaviour
{
    public GameObject prefabToDisable; // Reference to the prefab or prefab instance to disable
    public string targetSceneName; // The name of the scene where the prefab should be disabled

    private void Start()
    {
        // Subscribe to scene loading events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene matches the target scene
        if (scene.name == targetSceneName)
        {
            // Disable the prefab instance
            if (prefabToDisable != null)
            {
                prefabToDisable.SetActive(false);
                Debug.Log("Prefab disabled in scene: " + targetSceneName);
            }
            else
            {
                Debug.LogError("Prefab reference is null.");
            }
        }
        else
        {
            // Optionally, you might want to re-enable the prefab in other scenes
            if (prefabToDisable != null)
            {
                prefabToDisable.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loading events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
