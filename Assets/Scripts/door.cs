using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public float interactionDistance;
    public float rayRadius = 0.3f;
    public GameObject intText;

    public string doorOpenAnimName, doorCloseAnimName;
    public string doubleDoorLOpenAnim, doubleDoorLCloseAnim;
    public string doubleDoorROpenAnim, doubleDoorRCloseAnim;

    public AudioClip doorOpen, doorClose;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        RaycastHit hit;

        if (Physics.SphereCast(ray, rayRadius, out hit, interactionDistance))
        {
            if (hit.collider.gameObject.tag == "door")
            {
                Debug.Log("Hit Door");
                GameObject doorParent = hit.collider.transform.parent.gameObject;
                Debug.Log("PARENT NAME IS " + doorParent.name);
                Animator doorAnim = doorParent.GetComponent<Animator>();

                AudioSource doorSound = hit.collider.gameObject.GetComponent<AudioSource>();

                intText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E Button Pressed");

                    if (doorSound != null)
                    {
                        if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
                        {
                            doorSound.clip = doorClose;
                            doorSound.Play();
                            doorAnim.ResetTrigger("open");
                            doorAnim.SetTrigger("close");
                        }
                        else if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName))
                        {
                            doorSound.clip = doorOpen;
                            doorSound.Play();
                            doorAnim.ResetTrigger("close");
                            doorAnim.SetTrigger("open");
                        }
                        else if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorLOpenAnim))
                        {
                            doorSound.clip = doorClose;
                            doorSound.Play();
                            doorAnim.ResetTrigger("open");
                            doorAnim.SetTrigger("close");
                        }
                        else if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorLCloseAnim))
                        {
                            doorSound.clip = doorOpen;
                            doorSound.Play();
                            doorAnim.ResetTrigger("close");
                            doorAnim.SetTrigger("open");
                        }
                        else if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorROpenAnim))
                        {
                            doorSound.clip = doorClose;
                            doorSound.Play();
                            doorAnim.ResetTrigger("open");
                            doorAnim.SetTrigger("close");
                        }
                        else if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doubleDoorRCloseAnim))
                        {
                            doorSound.clip = doorOpen;
                            doorSound.Play();
                            doorAnim.ResetTrigger("close");
                            doorAnim.SetTrigger("open");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No AudioSource found on the door!");
                    }
                }
            }
            else
            {
                intText.SetActive(false);
            }
        }
        else
        {
            intText.SetActive(false);
        }
    }
}
