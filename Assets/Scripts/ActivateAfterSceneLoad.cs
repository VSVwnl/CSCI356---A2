using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateAfterSceneLoad : MonoBehaviour
{
    public GameObject targetGameObject; // The GameObject to be activated
    public float delay = 2f; // Delay in seconds before activation

    void Awake()
    {
        // Register to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unregister from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start the coroutine to activate the GameObject after a delay
        StartCoroutine(ActivateGameObjectAfterDelay(delay));
    }

    private IEnumerator ActivateGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetGameObject != null)
        {
            targetGameObject.SetActive(true); // Activate the GameObject and all its children
        }
    }
}
