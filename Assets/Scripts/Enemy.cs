using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Attack
    }

    private EnemyState currentState;
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject player;
    public Animator animator;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    public Path path;
    [Header("Sight Values")]
    public float sightDistance = 15f;
    public float fieldOfView = 40f;
    public float eyeHeight;
    [Header("Weapon Values")]
    public Transform gunBarrel;
    public GameObject gun;
 
    [Range(0.1f, 10f)]
    public float fireRate; // Rate of fire in bullets per second

    [Header("Health Values")]
    private EnemyHealth enemyHealth;

    [SerializeField]
    private string stateDescription;

    [Header("UI")]
    public GameObject enemyUIPrefab; // Reference to the EnemyUI prefab

    private bool isAttacking = false;
    public bool isHit = false;
    public bool isDead = false;
    private float nextFireTime = 0f; // Time when the enemy can fire next

    private Vector3 originalGunPosition;
    private Quaternion originalGunRotation;

    private int wayPointIndex = 0;
    public int enemyType = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        currentState = EnemyState.Patrol;

        // Instantiate and set up the enemy UI
        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
        }

        if (gun != null)
        {
            originalGunPosition = gun.transform.localPosition;
            originalGunRotation = gun.transform.localRotation;
        }
    }

    void Update()
    {
        if (isDead)
        {
            HandleDeadState();
            return;
        }

        if (!isHit) // Prevent other actions while hit animation is playing
        {
            switch (currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }

            stateDescription = currentState.ToString();
        }

        // Keep the UI always facing the camera
        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
            FaceCamera();
        }
    }

    private void HandleDeadState()
    {
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y - 0.99f, currentPosition.z);
        agent.transform.position = newPosition;
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                
                if(angleToPlayer < fieldOfView)
                // if(targetDirection.magnitude < distanceToPlayer)
                // if (angleToPlayer <= fieldOfView / 2)
                {
                    Ray ray = new Ray(transform.position, targetDirection.normalized);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject.CompareTag("Player"))
                        {
                            Debug.Log("Player detected within sight.");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    private void PositionUIAboveEnemy()
    {
        if (enemyUIPrefab != null)
        {
            // Adjust this value to position the UI above the enemy
            Vector3 offset = new Vector3(0, 2.5f, 0); // Example offset
            enemyUIPrefab.transform.localPosition = offset;
        }
    }

    private void FaceCamera()
    {
        if (enemyUIPrefab != null)
        {
            // Make the UI always face the camera
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                enemyUIPrefab.transform.LookAt(mainCamera.transform);
                // Ensure UI is not flipped when facing away from the camera
                enemyUIPrefab.transform.Rotate(Vector3.up * 180f);
            }
        }
    }

    private void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            if (animator != null)
            {
                animator.SetBool("isAttacking", true);
            }

            if (agent != null)
            {
                agent.isStopped = true;
            }

            if (gun != null)
            {
                switch (enemyType)
                {
                    case 1: // Nightshade type
                        gun.transform.localPosition = new Vector3(0.151f, 1.686f, 0.402f);
                        gun.transform.localRotation = Quaternion.Euler(7.086f, -176.454f, 0.763f);
                        break;
                    case 2: //Arissa type
                        gun.transform.localPosition = new Vector3(0.127f, 1.448f, 0.462f);
                        gun.transform.localRotation = Quaternion.Euler(8.544f, -176.435f, 0.766f);
                        break;
                    case 3: //Vampire type
                        gun.transform.localPosition = new Vector3(0.198f, 1.762f, 0.562f);
                        gun.transform.localRotation = Quaternion.Euler(11.561f, -196.286f, 1.868f);
                        break;
                    default:
                        gun.transform.localPosition = new Vector3(0.151f, 1.65f, 0.412f);
                        gun.transform.localRotation = Quaternion.Euler(15.733f, -176.335f, 0.787f);
                        break;
                }
            }
        }
    }

    private void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            if (animator != null)
            {
                animator.SetBool("isAttacking", false);
            }

            if (agent != null)
            {
                agent.isStopped = false;
            }

            if (gun != null)
            {
                gun.transform.localPosition = originalGunPosition;
                gun.transform.localRotation = originalGunRotation;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (!isHit)
        {
            isHit = true;
            Debug.Log("Taking damage, setting isHit to true");
            animator.SetBool("isHit", true);

            if (isAttacking)
            {
                animator.SetBool("isAttacking", false);
                isAttacking = false;
            }

            // Stop the NavMeshAgent to prevent movement
            if (agent != null)
            {
                agent.isStopped = true; // Stop the agent when hit
            }

            FacePlayer();

            if (gun != null)
            {
                gun.transform.localPosition = originalGunPosition;
                gun.transform.localRotation = originalGunRotation;
            }

            // Apply damage to the enemy's health
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy Health: " + enemyHealth.health); // Log the current health

                // Check if the enemy is dead
                if (enemyHealth.health <= 0 && !isDead)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(ResetHitState()); // Reset hit state if not dead
                }
            }
        }
    }

    private IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for hit animation to complete
        isHit = false;  // Reset isHit flag
        animator.SetBool("isHit", false);

        // Resume movement after the hit animation
        if (!isDead && agent != null)
        {
            agent.isStopped = false; // Resume the agent's movement
        }

        // Resume attack if the player is still in sight
        if (!isDead && CanSeePlayer())
        {
            StartAttack();
        }
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Rotate the enemy to face the player
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = lookRotation;
        }
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.2f)
        {
            if (wayPointIndex < path.waypoints.Count - 1)
            {
                wayPointIndex++;
            }
            else
            {
                wayPointIndex = 0;
            }
            agent.SetDestination(path.waypoints[wayPointIndex].position);
        }

        if (CanSeePlayer())
        {
            currentState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        if (CanSeePlayer())
        {
            if (!isAttacking)
            {
                Debug.Log("Enemy starting attack.");
                StartAttack();
            }

            if (Time.time >= nextFireTime)
            {
                Debug.Log("Enemy shooting.");
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            Debug.Log("Player no longer in sight, stopping attack.");
            StopAttack();
            currentState = EnemyState.Patrol;
        }
    }

    private void Shoot()
    {
        Debug.Log("Enemy firing bullet.");

        Transform gunbarrel = gunBarrel;
        GameObject bullet = Instantiate(Resources.Load("Bullet") as GameObject, gunbarrel.position, gunbarrel.rotation);
        Vector3 shootDirection = (player.transform.position - gunbarrel.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * 40;
    }

    public void Die()
    {
        if (isDead) { return; }

        isDead = true;
        Debug.Log("Enemy Died!");

        if (animator != null)
        {
            animator.SetBool("isDead", true); // Set the isDead boolean to true
        }

        if (enemyUIPrefab != null)
        {
            enemyUIPrefab.gameObject.SetActive(false);
        }

        // Stop the NavMeshAgent to prevent further movement
        if (agent != null)
        {
            agent.isStopped = true;
        }

        PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth();
        }

    }


    // Draw Gizmos to visualize the enemy's sight distance and field of view
    private void OnDrawGizmos()
    {
        // Draw the sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        // Draw the field of view as lines
        Gizmos.color = Color.red;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView, 0) * transform.forward * sightDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView, 0) * transform.forward * sightDistance;
        Gizmos.DrawLine(transform.position + Vector3.up * eyeHeight, transform.position + rightBoundary + Vector3.up * eyeHeight);
        Gizmos.DrawLine(transform.position + Vector3.up * eyeHeight, transform.position + leftBoundary + Vector3.up * eyeHeight);
    }
}