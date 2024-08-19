using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int wayPointIndex;
    public override void Enter()
    {

    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
 
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            if (wayPointIndex < enemy.path.waypoints.Count - 1)
                wayPointIndex++;
            else
            {
                wayPointIndex = 0;
            }
            enemy.Agent.SetDestination(enemy.path.waypoints[wayPointIndex].position);
        }
    }
}
