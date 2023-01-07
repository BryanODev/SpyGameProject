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
    NavMeshAgent navAgent;

    public Transform eyeSight;
    public LayerMask playerMask;
    public EyeSightRayData[] EyeSightRays;

    Transform chaseTarget;
    bool IsChasing;

    Vector3 startPos;
    Quaternion startRot;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        eyeSight = transform.GetChild(0);
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!IsChasing)
        {
            for (int i = 0; i < EyeSightRays.Length; i++)
            {
                EyeSightRay(EyeSightRays[i].StartXOffset);
            }
        }
        else 
        {
            navAgent.SetDestination(chaseTarget.position);

            if (Vector3.Distance(transform.position, chaseTarget.position) > 3.0f) 
            {
                IsChasing = false;
                chaseTarget = null;

                navAgent.SetDestination(startPos);
            }
        }

        if (navAgent) 
        {
            transform.rotation = startRot;
        }
    }

    void EyeSightRay(float xOffset) 
    {
        RaycastHit hit;
        Vector3 startPos = eyeSight.position;
        startPos.x += xOffset;

        Vector3 DirectionPos = eyeSight.forward;
        DirectionPos.x += xOffset;

        Debug.DrawRay(startPos, DirectionPos * 5.0f, Color.magenta);

        if (Physics.Raycast(startPos, DirectionPos, out hit, 5.0f, playerMask))
        {
            Debug.Log("Stop right there!");

            chaseTarget = hit.transform;
            IsChasing = true;
        }
    }
}
