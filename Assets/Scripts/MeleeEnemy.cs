using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Attack
    }

    private EnemyState currentState;
    private NavMeshAgent agent;
    private GameObject player;
    public Animator animator;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    public Path path;
    [Header("Melee Values")]
    public float attackRadius = 3f;
    public int attackRange = 2;
    public int meleeDamage = 10;

    [Header("Health Values")]
    private EnemyHealth enemyHealth;

    [SerializeField]
    private string stateDescription;

    [Header("UI")]
    public GameObject enemyUIPrefab;

    private bool isAttacking = false;
    public bool isHit = false;
    public bool isDead = false;

    private Coroutine attackCoroutine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        currentState = EnemyState.Patrol;

        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
        }
    }

    void Update()
    {
        if (isDead)
        {
            HandleDeadState();
            return;
        }

        if (!isHit)
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

        if (enemyUIPrefab != null)
        {
            PositionUIAboveEnemy();
            FaceCamera();
        }
    }

    private void HandleDeadState()
    {
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y - 0.5f, currentPosition.z);
        agent.transform.position = newPosition;
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

            if (agent != null)
            {
                agent.isStopped = true;
            }

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(ApplyDamageWhileAttacking());
        }
    }

    private IEnumerator ApplyDamageWhileAttacking()
    {
        if (animator != null)
        {
            float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            float delay = attackAnimationLength / animator.speed;

            yield return new WaitForSeconds(delay);

            while (isAttacking && IsPlayerWithinAttackRadius())
            {
                if (player != null)
                {
                    PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(meleeDamage);
                        Debug.Log("Damage applied to the player.");
                    }
                    else
                    {
                        Debug.LogWarning("PlayerHealth component not found on the player or its parent.");
                    }
                }

                yield return new WaitForSeconds(delay);
            }

            StopAttack();
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
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (!isHit)
        {
            isHit = true;
            Debug.Log("Taking damage, setting isHit to true");

            // Set hit animation
            if (animator != null)
            {
                animator.SetBool("isHit", true);
            }

            // Stop attack if currently attacking
            if (isAttacking)
            {
                StopAttack();
            }

            // Stop agent movement
            if (agent != null)
            {
                agent.isStopped = true;
            }

            // Apply damage to enemy health
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

                if (enemyHealth.health <= 0 && !isDead)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(ResetHitState());
                }
            }
        }
    }

    private IEnumerator ResetHitState()
    {
        float hitAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(hitAnimationLength);

        isHit = false;
        Debug.Log("Resetting isHit to false");
        if (animator != null)
        {
            animator.SetBool("isHit", false);
        }

        if (!isDead && agent != null)
        {
            agent.isStopped = false;
        }

        if (!isDead && IsPlayerWithinAttackRadius())
        {
            StartAttack();
        }
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.2f)
        {
            int wayPointIndex = Random.Range(0, path.waypoints.Count);
            agent.SetDestination(path.waypoints[wayPointIndex].position);
        }

        if (IsPlayerWithinAttackRadius())
        {
            currentState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                StartAttack();
            }

            if (distanceToPlayer > attackRange)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
        else
        {
            StopAttack();
            currentState = EnemyState.Patrol;
        }
    }

    private bool IsPlayerWithinAttackRadius()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= attackRadius;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy Died!");

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}