using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    private NavMeshAgent agent;
    public override void Enter()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true; // Stop the NavMeshAgent
        }
    }

    public override void Exit()
    {
        if (agent != null)
        {
            agent.isStopped = false; // Resume movement when exiting the attack state
        }
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) // can see player
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            // if shot timer > fireRate
            if (!enemy.isHit && shotTimer > enemy.fireRate)
            {
                Shoot();
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                // change to search state
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * 40;

        Debug.Log("Shoot");
        shotTimer = 0;
    }
}