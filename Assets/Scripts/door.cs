using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    //If you already have your own raycast script, feel free to integrate whatever you need from this door script into your current raycast script :)

    //Distance from which the player can interact with the door
    public float interactionDistance;
    public float rayRadiuus = 0.3f;
    //The text that appears to let you know you can interact with the door
    public GameObject intText;

    //The names of the door open and door close animations
    public string doorOpenAnimName, doorCloseAnimName;
    public string doubleDoorLOpenAnim, doubleDoorLCloseAnim;
    public string doubleDoorROpenAnim, doubleDoorRCloseAnim;

    //The door open and door close sounds
    public AudioClip doorOpen, doorClose;


    //The Update() void is where stuff occurs every frame
    void Update()
    {
        //A ray is created which will shoot forward from the player's camera
        Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        //RaycastHit variable, which is used to get info back from whatever the raycast hits
        RaycastHit hit;

        //If the raycast hits something
        if (Physics.SphereCast(ray, rayRadiuus ,out hit, interactionDistance))
        {
            //If the object the raycast hits is tagged as door
            if (hit.collider.gameObject.tag == "door")
            {
                Debug.Log("Hit Door");
                //A GameObject variable is created for the door's main parent object
                GameObject doorParent = hit.collider.transform.parent.gameObject;
                Debug.Log("PARENT NAME IS " +  doorParent.name);
                //An Animator variable is created for the doorParent's Animator component
                Animator doorAnim = doorParent.GetComponent<Animator>();

                //An AudioSource variable is created for the door's Audio Source component
                //AudioSource doorSound = hit.collider.gameObject.GetComponent<AudioSource>();

                //The interaction text is set active
                intText.SetActive(true);

                //If the E key is pressed
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E BUtton Press");
                    //If the door's Animator's state is set to the open animation
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
                    {
                        //The doorSound's audio clip will change to the door close sound
                        //doorSound.clip = doorClose;
                        Debug.Log("Opening Door)");
                        //The door close sound will play
                        //doorSound.Play();

                        //The door's open animation trigger is reset
                        doorAnim.ResetTrigger("open");

                        //The door's close animation trigger is set (it plays)
                        doorAnim.SetTrigger("close");
                    }
                    //If the door's Animator's state is set to the close animation
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName))
                    {
                        //The doorSound's audio clip will change to the door open sound
                        //doorSound.clip = doorOpen;

                        //The door open sound will play
                        //doorSound.Play();

                        //The door's close animation trigger is reset
                        doorAnim.ResetTrigger("close");

                        //The door's open animation trigger is set (it plays)
                        doorAnim.SetTrigger("open");
                    }
                    //If the door's Animator's state is set to the open animation
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorLOpenAnim))
                    {
                        //The doorSound's audio clip will change to the door close sound
                        //doorSound.clip = doorClose;
                        Debug.Log("Opening Door)");
                        //The door close sound will play
                        //doorSound.Play();

                        //The door's open animation trigger is reset
                        doorAnim.ResetTrigger("open");

                        //The door's close animation trigger is set (it plays)
                        doorAnim.SetTrigger("close");
                    }
                    //If the door's Animator's state is set to the close animation
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorLCloseAnim))
                    {
                        //The doorSound's audio clip will change to the door open sound
                        //doorSound.clip = doorOpen;

                        //The door open sound will play
                        //doorSound.Play();

                        //The door's close animation trigger is reset
                        doorAnim.ResetTrigger("close");

                        //The door's open animation trigger is set (it plays)
                        doorAnim.SetTrigger("open");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorROpenAnim))
                    {
                        //The doorSound's audio clip will change to the door close sound
                        //doorSound.clip = doorClose;
                        Debug.Log("Opening Door)");
                        //The door close sound will play
                        //doorSound.Play();

                        //The door's open animation trigger is reset
                        doorAnim.ResetTrigger("open");

                        //The door's close animation trigger is set (it plays)
                        doorAnim.SetTrigger("close");
                    }
                    //If the door's Animator's state is set to the close animation
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorRCloseAnim))
                    {
                        //The doorSound's audio clip will change to the door open sound
                        //doorSound.clip = doorOpen;

                        //The door open sound will play
                        //doorSound.Play();

                        //The door's close animation trigger is reset
                        doorAnim.ResetTrigger("close");

                        //The door's open animation trigger is set (it plays)
                        doorAnim.SetTrigger("open");
                    }
                }
            }
            //else, if not looking at the door
            else
            {
                //The interaction text is disabled
                intText.SetActive(false);
            }
        }
        //else, if not looking at anything
        else
        {
            //The interaction text is disabled
            intText.SetActive(false);
            
        }
    }
}