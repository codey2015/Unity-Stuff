using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMovementSimple : MonoBehaviour {

    public Transform destination2;

    NavMeshAgent agent;
    // Use this for initialization
    public void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nextDestination();
    }

    private void nextDestination()
    {
        agent.destination = destination2.transform.position;
    }


}
 