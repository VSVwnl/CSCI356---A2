using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectCollector : MonoBehaviour
{
    public string collectibleTag = "Collectible";  // Tag to identify collectible objects
    public int totalCollected = 0;                // Number of objects collected
    public int requiredCollectibles = 12;         // Required number of collectibles to win
    public Text collectibleCounter;               // Reference to a UI Text element
    public Text pickUpPrompt;                     // Reference to the UI Text for the "E" prompt

    // List to store the names of collected items
    public List<string> inventory = new List<string>();

    void Start()
    {
        // Initialize the collectible counter text
        collectibleCounter.text = totalCollected + " / " +
             + requiredCollectibles +  " Talismans Found";

        // Initially hide the pick-up prompt
        pickUpPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        // Perform the raycast from the camera to the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the raycast hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has the collectible tag
            if (hit.transform.CompareTag(collectibleTag))
            {
                // Show the pick-up prompt when aiming at a collectible
                pickUpPrompt.gameObject.SetActive(true);

                // Check if the player presses the 'E' key to collect the item
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Get a reference to the collectible item
                    GameObject collectibleItem = hit.transform.gameObject;

                    // Add the collectible item's name to the inventory
                    inventory.Add(collectibleItem.name);

                    // Destroy the object in the scene
                    Destroy(collectibleItem);

                    // Increase the total collected count
                    totalCollected++;

                    // Update the collectible counter text
                    collectibleCounter.text = totalCollected + " / " + requiredCollectibles + " Talismans Found";

                    // Check if the player has collected the required number of items
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

    // Method to get the collected items from the inventory
    public List<string> GetInventory()
    {
        return inventory;
    }
}
