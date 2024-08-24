using UnityEngine;
using UnityEngine.UI;

public class ObjectCollector : MonoBehaviour
{
    public string collectibleTag = "Collectible";  // Tag to identify collectible objects
    public int totalCollected = 0;                // Number of objects collected
    public int requiredCollectibles = 12;          // Required number of collectibles to win
    public Text collectibleCounter;               // Reference to a UI Text element
    public Text pickUpPrompt;                     // Reference to the UI Text for the "E" prompt

    void Start()
    {
        // Initialize the collectible counter text
        collectibleCounter.text = totalCollected + " / " + requiredCollectibles;

        // Initially hide the pick-up prompt
        pickUpPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        // Perform the raycast from the camera to the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(collectibleTag))
            {
                // Show the pick-up prompt when aiming at a collectible
                pickUpPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    totalCollected++;
                    Destroy(hit.transform.gameObject);

                    // Update the collectible counter text
                    collectibleCounter.text = totalCollected + " / " + requiredCollectibles;

                    if (totalCollected >= requiredCollectibles)
                    {
                        Debug.Log("All items collected!");
                        // Implement what happens when all items are collected
                    }
                }
            }
            else
            {
                // Hide the pick-up prompt if not aiming at a collectible
                pickUpPrompt.gameObject.SetActive(false);
            }
        }
        else
        {
            // Hide the pick-up prompt if not aiming at anything
            pickUpPrompt.gameObject.SetActive(false);
        }
    }
}
