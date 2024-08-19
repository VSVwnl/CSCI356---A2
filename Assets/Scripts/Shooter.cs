using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera cam; // stores camera component

    public float impulseStrength;
    public GameObject particleSysPrefab;
    public GameObject bulletPrefab;
    public float bulletImpulse = 20.0f;

    private bool isShooting = false;    // Flag to track continuous fire
    private float fireDelay = 0.2f;     // Delay between shots
    private float lastFireTime;         // Time of the last shot

    public int weaponType = 1;         // Set weapon to 1.
    public GameObject[] hiddenObjects; // Array of hidden objects to activate based on weapon chosen


    // Start is called before the first frame update
    void Start()
    {
        // gets the GameObject's camera component
        cam = GetComponentInChildren<Camera>();

        // hide the mouse cursor at the centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set and display the first weapon on start
        weaponType = 1;
        changeWeapon(weaponType);
    }

    public void CycleWeapons()
    {
        weaponType++;

        if (weaponType > hiddenObjects.Length)
        {
            weaponType = 1; // Reset to first weapon if exceeding available weapons
        }

        changeWeapon(weaponType);
    }
    public void changeWeapon(int weapon)
    {
        if (weapon >= 1 && weapon <= hiddenObjects.Length)
        {
            weaponType = weapon;

            // Activate the hidden objects associated with the chosen weapon
            for (int i = 0; i < hiddenObjects.Length; i++)
            {
                hiddenObjects[i].SetActive(i + 1 == weapon); // Activate the object if it corresponds to the chosen weapon
            }
        }
    }

    void OnGUI()
    {
        int size = 12;

        // centre of screen and caters for font size
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;

        // displays "*" on screen
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleWeapons();
        }

        // for handgun
        if (weaponType == 1)
        {
            // on left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                // get point in the middle of the screen
                Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
                
                // Apply random offset to the point
                point.x += Random.Range(-5f, 5f);
                point.y += Random.Range(-5f, 5f);

                // create a ray from the point in the direction of the camera
                Ray ray = cam.ScreenPointToRay(point);

                RaycastHit hit; // stores ray intersection information

                // ray cast will obtain hit information if it intersects anything
                if (Physics.Raycast(ray, out hit))
                {
                    // get the GameObject that was hit
                    GameObject hitObject = hit.transform.gameObject;

                    // get Shootable component
                    Shootable target = hitObject.GetComponent<Shootable>();

                    // if the object has a Shootable component
                    if (target != null)
                    {
                        // calculate impulse
                        Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;

                        // add the impulse to the rigidbody 
                        hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);

                        // reduce health of the Shootable object
                        target.ApplyDamage(10);

                        // start coroutine to generate a bullet 
                        StartCoroutine(GenerateBullet(hit, point));
                    }

                }
            }
        }
        // for machine gun
        else if (weaponType == 2)
        {
            // on left mouse button down
            if (Input.GetMouseButtonDown(0))
            {
                isShooting = true; // Start continuous fire
            }

            // on left mouse button up
            if (Input.GetMouseButtonUp(0))
            {
                isShooting = false; // Stop continuous fire
            }

            // Continuous fire with delay executes if:
            // 1. isShooting is true
            // 2. a certain time has passed since the last shot (controlled by fireDelay).
            if (isShooting && Time.time - lastFireTime >= fireDelay)
            {
                // get point in the middle of the screen
                Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

                // Apply random offset to the point
                point.x += Random.Range(-5f, 5f);
                point.y += Random.Range(-5f, 5f);

                // a ray is created from the center of the screen to determine where the player is aiming
                Ray ray = cam.ScreenPointToRay(point);

                // stores ray intersection information
                RaycastHit hit;

                // the ray is cast
                // if it hits an object with a Shootable component, it applies an impulse force to the object, reduces its health, and generates a particle effect
                if (Physics.Raycast(ray, out hit))
                {
                    // get the GameObject that was hit
                    GameObject hitObject = hit.transform.gameObject;

                    // get Shootable component
                    Shootable target = hitObject.GetComponent<Shootable>();

                    // if the object has a Shootable component
                    if (target != null)
                    {
                        // calculate impulse
                        Vector3 impulse = Vector3.Normalize(hit.point - transform.position)
                            * impulseStrength;

                        // add the impulse to the rigidbody 
                        hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);

                        // reduce health of the Shootable object
                        target.ApplyDamage(15); 

                        // start coroutine to generate a particle system 
                        StartCoroutine(GeneratePS(hit));

                        // start coroutine to generate a bullet 
                        StartCoroutine(GenerateBullet(hit, point));
                    }
                }
                // Update the time of the last shot
                lastFireTime = Time.time;
            }
        }
        // for shotgun
        else if (weaponType == 3)
        {
            // on left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                // get point in the middle of the screen
                Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

                // Apply random offset to the point
                point.x += Random.Range(-5f, 5f);
                point.y += Random.Range(-5f, 5f);

                // create a ray from the point in the direction of the camera
                Ray ray = cam.ScreenPointToRay(point);

                RaycastHit hit; // stores ray intersection information

                // ray cast will obtain hit information if it intersects anything
                if (Physics.Raycast(ray, out hit))
                {
                    // get the GameObject that was hit
                    GameObject hitObject = hit.transform.gameObject;

                    // get Shootable component
                    Shootable target = hitObject.GetComponent<Shootable>();

                    // if the object has a Shootable component
                    if (target != null)
                    {
                        // calculate impulse
                        Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;

                        // add the impulse to the rigidbody 
                        hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);

                        // reduce health of the Shootable object
                        target.ApplyDamage(20); 

                        // start coroutine to generate a particle system 
                        StartCoroutine(GenerateAlotBullet(hit, point));
                    }

                }
            }
        }
    }


    private IEnumerator GeneratePS(RaycastHit hit)
    {
        // instantiate particle system prefab
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));

        // wait for 1 second
        yield return new WaitForSeconds(1);

        // remove the particle system
        Destroy(ps);
    }

    private IEnumerator GenerateBullet(RaycastHit hit, Vector3 point)
    {
        // instantiate bullet object prefab
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = cam.transform.position + cam.transform.forward * 2;

        // get the object's rigidbody component
        Rigidbody target = bullet.GetComponent<Rigidbody>();

        // calculate impulse strength
        Vector3 impulse = cam.transform.forward * bulletImpulse;

        // apply impulse with a random offset for accuracy
        impulse += Random.Range(-0.1f, 0.1f) * cam.transform.right;
        impulse += Random.Range(-0.1f, 0.1f) * cam.transform.up;

        // apply impulse
        target.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);

        // wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // remove the bullet
        Destroy(bullet);
    }

    private IEnumerator GenerateAlotBullet(RaycastHit hit, Vector3 point)
    {
        GameObject bullet = Instantiate(bulletPrefab);

        for (int i = 0; i < 7; i++)
        {
            // instantiate bullet object prefab
            bullet = Instantiate(bulletPrefab);
            bullet.transform.position = cam.transform.position + cam.transform.forward * 2;

            // get the object's rigidbody component
            Rigidbody target = bullet.GetComponent<Rigidbody>();

            // calculate impulse strength
            Vector3 impulse = cam.transform.forward * bulletImpulse;

            // apply impulse with a random offset for accuracy
            impulse += Random.Range(-0.1f, 0.1f) * cam.transform.right;
            impulse += Random.Range(-0.1f, 0.1f) * cam.transform.up;

            // apply impulse
            target.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);
        }

        // wait for a short delay
        yield return new WaitForSeconds(0.5f);

        // remove the bullet
        Destroy(bullet);
    }
}