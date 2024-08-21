using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
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
    [Range(0.1f, 10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;

    [Header("UI")]
    public GameObject enemyUIPrefab;

    private bool isAttacking = false;
    public bool isHit = false;
    public bool isDead = false; // New flag for death state

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit && !isDead) // Prevent other actions while hit or death animation is playing
        {
            bool playerInSight = CanSeePlayer();

            if (playerInSight)
            {
                StartAttack();
            }
            else
            {
                StopAttack();
            }

            currentState = stateMachine.activeState.ToString();

            if (enemyUIPrefab != null)
            {
                PositionUIAboveEnemy();
                FaceCamera();
            }
        }
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            // Calculate the distance between the enemy and the player using Vector3.Distance
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within sight distance
            if (distanceToPlayer < sightDistance)
            {
                // Calculate the direction to the player
                Vector3 targetDirection = (player.transform.position - transform.position).normalized;

                // Get the angle between the forward direction of the enemy and the direction to the player
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                // Check if the player is within the field of view
                if (angleToPlayer <= fieldOfView / 2) // Dividing by 2 because it's an angle on either side
                {
                    // Perform a raycast to check if there are any obstacles between the enemy and the player
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        // If the ray hits the player, return true
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.green); // Debug the ray
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
            Vector3 offset = new Vector3(0, 2.5f, 0);
            enemyUIPrefab.transform.localPosition = offset;
        }
    }

    private void FaceCamera()
    {
        if (enemyUIPrefab != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                enemyUIPrefab.transform.LookAt(mainCamera.transform);
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
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Prevent damage processing if already dead

        if (!isHit)
        {
            isHit = true;
            animator.SetBool("isHit", true);

            if (isAttacking)
            {
                animator.SetBool("isAttacking", false);
                isAttacking = false;
            }

            // Stop the NavMeshAgent to prevent movement when hit
            if (agent != null)
            {
                agent.isStopped = true;
            }

            Shootable shootable = GetComponent<Shootable>();
            if (shootable != null)
            {
                shootable.ApplyDamage(damage);

                if (shootable.GetHealth() <= 0)
                {
                    Die(); // Trigger death
                    return; // Prevent further damage processing
                }
            }

            StartCoroutine(ResetHitState());
        }
    }

    public void Die()
    {
        if (isDead) return; // Prevent multiple calls to Die

        isDead = true; // Set death flag
        animator.SetTrigger("Die"); // Trigger death animation

        // Disable enemy functionality (stop movement, etc.)
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // Optionally, disable or remove other components related to AI behavior
        if (stateMachine != null)
        {
            stateMachine.enabled = false;
        }

        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Wait for death animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the enemy after the animation finishes
        Destroy(gameObject);
    }

    private IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isHit = false;
        animator.SetBool("isHit", false);

        if (agent != null)
        {
            agent.isStopped = false; // Resume movement
        }

        if (CanSeePlayer() && !isDead)
        {
            StartAttack();
        }
    }
}