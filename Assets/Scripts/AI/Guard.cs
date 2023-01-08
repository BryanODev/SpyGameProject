using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EyeSightRayData 
{
    public float StartXOffset;
}

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
    protected NavMeshAgent navAgent;
    public float walkSpeed = 2.0f;
    public float chaseSpeed = 3.0f;

    public Transform eyeSight;
    public LayerMask playerMask;
    public EyeSightRayData[] EyeSightRays;


    Transform chaseTarget;
    public float chaseDistance = 5.0f;
    [SerializeField] protected bool IsChasing;
    public bool canLookForTarget = true;
    public bool isPatrolling;

    Vector3 positionBeforeChase;
    Quaternion startRot;

    Animator guardAnimator;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        eyeSight = transform.GetChild(0);
        navAgent.speed = walkSpeed;

        guardAnimator = GetComponentInChildren<Animator>();
    }

    public virtual void Update()
    {
        guardAnimator.SetFloat("Speed", navAgent.velocity.magnitude);


        if (!IsChasing)
        {
            navAgent.stoppingDistance = 0;
            if (canLookForTarget)
            {
                //If we arent chasing, we gonna check whather theres something in front of us using the EyeSightRays
                for (int i = 0; i < EyeSightRays.Length; i++)
                {
                    EyeSightRay(EyeSightRays[i].StartXOffset);
                }
            }

            if (DestinationReached()) 
            {
                if (!isPatrolling)
                {
                    transform.rotation = startRot;
                }

                canLookForTarget = true;
            }
        }
        

        if(IsChasing)
        {
            navAgent.stoppingDistance = 1.5f;
            navAgent.SetDestination(chaseTarget.position);

            //If we run away in a safe distance of the guard, it will stop chasing us and go back to the starting point
            if (Vector3.Distance(transform.position, chaseTarget.position) > chaseDistance) 
            {
                IsChasing = false;
                chaseTarget = null;

                navAgent.SetDestination(positionBeforeChase);
            }

            if (DestinationReached()) 
            {
                StartCoroutine(Capture());
            }
        }
    }

    void EyeSightRay(float xOffset) 
    {
        RaycastHit hit;
        Vector3 startPos = eyeSight.position;

        Vector3 DirectionPos = eyeSight.forward;
        DirectionPos = Quaternion.Euler(0, xOffset, 0) * DirectionPos;

        Debug.DrawRay(startPos, DirectionPos * 5.0f, Color.magenta);

        if (Physics.Raycast(startPos, DirectionPos, out hit, 5.0f, playerMask))
        {
            SetChaseTarget(hit.transform);
            IsChasing = true;
            positionBeforeChase = transform.position;
            startRot = transform.rotation;
            navAgent.speed = chaseSpeed;
        }
    }

    public void SetChaseTarget(Transform newTarget) 
    {
        chaseTarget = newTarget;
    }

    public void SetNavDestination(Transform point) 
    {
        navAgent.SetDestination(point.position);
    }

    public bool DestinationReached() 
    {
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator Capture() 
    {
        yield return new WaitForSeconds(.25f);

        Debug.Log("We capture the player!");

        canLookForTarget = false;
        IsChasing = false;
        chaseTarget = null;
        navAgent.SetDestination(positionBeforeChase);

        //Tell the gamemode we caught the player
        GameMode.Instance.OnPlayerCaugh();

        yield return null;
    }
}
