using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMovementArray : MonoBehaviour
{
    public Transform[] destination2;

    NavMeshAgent agent;
    private int destPoint;
    // Use this for initialization
    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        destPoint = (destPoint + 1) % destination2.Length;
    }

    public void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            nextDestination();
        }
    }

    private void nextDestination()
    {
        agent.destination = destination2[destPoint].position;
        destPoint += 1;
    }


}