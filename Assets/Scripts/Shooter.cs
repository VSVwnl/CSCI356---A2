using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private TalismanHighlight lastHighlightedTalisman; // Reference to the last highlighted talisman

    // Start is called before the first frame update
    void Start()
    {
        // gets the GameObject's camera component
        cam = GetComponentInChildren<Camera>();

        // hide the mouse cursor at the centre of the screen
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
        // Weapon switching logic
        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleWeapons();
        }

        // Raycasting for charm highlighting (regardless of weapon type)
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TalismanHighlight talisman = hit.transform.GetComponent<TalismanHighlight>();

            if (talisman != null)
            {
                talisman.Highlight(true);
                lastHighlightedTalisman = talisman;
            }
            else if (lastHighlightedTalisman != null)
            {
                lastHighlightedTalisman.Highlight(false);
                lastHighlightedTalisman = null;
            }
        }

        // Weapon logic for handgun
        if (weaponType == 1 && Input.GetMouseButtonDown(0))
        {
            HandleHandgunFire(ray);
        }
        // Weapon logic for machine gun
        else if (weaponType == 2)
        {
            if (Input.GetMouseButtonDown(0)) isShooting = true;
            if (Input.GetMouseButtonUp(0)) isShooting = false;

            if (isShooting && Time.time - lastFireTime >= fireDelay)
            {
                HandleMachineGunFire(ray);
                lastFireTime = Time.time;
            }
        }
        // Weapon logic for shotgun
        else if (weaponType == 3 && Input.GetMouseButtonDown(0))
        {
            HandleShotgunFire(ray);
        }
    }

    void HandleHandgunFire(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Shootable target = hit.transform.GetComponent<Shootable>();

            if (target != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                target.ApplyDamage(5);
                StartCoroutine(GenerateBullet(hit, ray.origin));
            }
        }
    }

    void HandleMachineGunFire(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Shootable target = hit.transform.GetComponent<Shootable>();

            if (target != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                target.ApplyDamage(10);
                StartCoroutine(GeneratePS(hit));
                StartCoroutine(GenerateBullet(hit, ray.origin));
            }
        }
    }

    void HandleShotgunFire(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Shootable target = hit.transform.GetComponent<Shootable>();

            if (target != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                target.ApplyDamage(15);
                StartCoroutine(GenerateAlotBullet(hit, ray.origin));
            }
        }
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }

    private IEnumerator GenerateBullet(RaycastHit hit, Vector3 point)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = cam.transform.position + cam.transform.forward * 2;
        Rigidbody target = bullet.GetComponent<Rigidbody>();
        Vector3 impulse = cam.transform.forward * bulletImpulse;
        impulse += Random.Range(-0.1f, 0.1f) * cam.transform.right;
        impulse += Random.Range(-0.1f, 0.1f) * cam.transform.up;
        target.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet);
    }

    private IEnumerator GenerateAlotBullet(RaycastHit hit, Vector3 point)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = cam.transform.position + cam.transform.forward * 2;
            Rigidbody target = bullet.GetComponent<Rigidbody>();
            Vector3 impulse = cam.transform.forward * bulletImpulse;
            impulse += Random.Range(-0.1f, 0.1f) * cam.transform.right;
            impulse += Random.Range(-0.1f, 0.1f) * cam.transform.up;
            target.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);
            yield return new WaitForSeconds(0.05f);  // Short delay between bullets
        }
    }
}
