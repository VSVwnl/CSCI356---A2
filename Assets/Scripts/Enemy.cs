using System.Collections;
using System.Collections.Generic;
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
    public GameObject enemyUIPrefab; // Reference to the EnemyUI prefab

    private bool isAttacking = false;
    public bool isHit = false;

    // Start is called before the first frame update
    void Awake()
    {

        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        // Instantiate and set up the enemy UI
        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit) // Prevent other actions while hit animation is playing
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

            // Keep the UI always facing the camera
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
            if (Vector2.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
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

            Shootable shootable = GetComponent<Shootable>();
            if (shootable != null)
            {
                shootable.ApplyDamage(damage);
            }

            StartCoroutine(ResetHitState());
        }
    }

    private IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for hit animation to complete
        isHit = false;  // Reset isHit flag
        animator.SetBool("isHit", false);

        // Resume attack if the player is still in sight
        if (CanSeePlayer())
        {
            StartAttack();
        }
    }
}