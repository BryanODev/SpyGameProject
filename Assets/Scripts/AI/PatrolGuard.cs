using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGuard : Guard
{
    public Transform[] patrolWayPoints;
    public int currentWayPointIndex;
    bool ReachedDestination;

    public void Start()
    {
        SetNavDestination(patrolWayPoints[0]);
        isPatrolling = true;
    }

    public override void Update()
    {
        base.Update();

        if (!IsChasing)
        {
            Patrol();
        }
    }

    void Patrol() 
    {
        if (DestinationReached()) 
        {
            currentWayPointIndex += 1;

            if (currentWayPointIndex == patrolWayPoints.Length) 
            {
                currentWayPointIndex = 0;
            }

            SetNavDestination(patrolWayPoints[currentWayPointIndex]);
        }
    }
}
