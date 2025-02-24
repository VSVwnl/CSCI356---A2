using System.Collections;
using System.Linq;
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
    public bool isReloading = false;
    private float fireDelay = 0.1f;     // Delay between shots
    private float lastFireTime;         // Time of the last shot

    public int weaponType = 1;         // Set weapon to 1.
    public GameObject[] hiddenObjects; // Array of hidden objects to activate based on weapon chosen

    private TalismanHighlight lastHighlightedTalisman; // Reference to the last highlighted talisman

    // Ammunition tracking
    public int maxAmmunition = 10;     // Maximum bullets in the magazine
    private int currentAmmunition;      // Current bullets available
    private int currentPistolAmmo;      // Current Pistol available
    private int currentARAmmo;          // Current AR Ammo Available
    public int MaxARAMMO = 30;          // Max AR Ammo
    public int MaxPistolAMMO = 10;      // Max Pistol Ammo

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

        // Initialize ammunition
        //currentAmmunition = maxAmmunition;
        currentPistolAmmo = MaxPistolAMMO;
        currentARAmmo = MaxARAMMO;
    }

    public void CycleWeapons()
    {
        if (!isReloading)
        {

            weaponType++;

            if (weaponType > 2)
            {
                weaponType = 1; // Reset to first weapon if exceeding available weapons
            }


            changeWeapon(weaponType);

        }
        else
        {
            return;
        }
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
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon switching logic

        Debug.Log("Current Weapon Is " + weaponType);
        // Debug.Log("AMMO FOR PISTOL IS " + currentPistolAmmo);
        //Debug.Log("AMMO FOR AR IS " + currentARAmmo);
        Debug.Log("isReloading = " + isReloading);
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
        if (weaponType == 2 && Input.GetMouseButtonDown(0) && !isReloading)
        {
            HandleHandgunFire(ray);
        }
        // Weapon logic for machine gun
        else if (weaponType == 1)
        {
            if (Input.GetMouseButtonDown(0)) isShooting = true;
            if (Input.GetMouseButtonUp(0)) isShooting = false;

            if (isShooting && Time.time - lastFireTime >= fireDelay && !isReloading)
            {
                HandleMachineGunFire(ray);
                lastFireTime = Time.time;
            }
        }

        // Reloading logic
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void HandleHandgunFire(Ray ray)
    {
        if (currentPistolAmmo == 0)
        {
            // Optionally, play an empty ammo sound or show a UI indicator
            Debug.Log("NO AMMO FOR PISTOL");
            return; // No more ammo to fire
        }
        currentPistolAmmo--;


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
        if (currentARAmmo == 0)
        {
            // Optionally, play an empty ammo sound or show a UI indicator
            Debug.Log("NO AMMO FOR AR");
            return; // No more ammo to fire
        }
        currentARAmmo--;
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
        // Removed shotgun logic
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        // Only generate particles if there is ammunition
        if (currentPistolAmmo > 0 || currentARAmmo > 0)
        {
            GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            yield return new WaitForSeconds(1);
            Destroy(ps);
        }
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
            if (currentAmmunition <= 0)
                yield break; // Exit if out of ammunition

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
    private IEnumerator ReloadReset()
    {
        isReloading = true;
        yield return new WaitForSeconds(2.3f);
        isReloading = false;

    }

    // Optional: Method to reload and refill ammunition
    public void Reload()
    {
        StartCoroutine(ReloadReset());
        //currentAmmunition = maxAmmunition;
        if (weaponType == 1)
        {
            currentARAmmo = MaxARAMMO;
        }
        else if (weaponType == 2)
        {
            currentPistolAmmo = MaxPistolAMMO;
        }

    }
}
