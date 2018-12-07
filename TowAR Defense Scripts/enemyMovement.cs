using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class enemyMovement : MonoBehaviour {

	private Transform target;
	private int wavepointIndex = 0;
	public float damage = 50f;

    public int trailID = -1;
    private Enemy enemy;
    private Waypoints currentPath;
	// Use this for initialization
	void Start () 
	{
		enemy = GetComponent<Enemy>();
        Waypoints[] tempTrail = GameObject.FindObjectsOfType<Waypoints>();
        int trailCount = 0;
        foreach (Waypoints trail in tempTrail)
        {

            if(trail.trailID == trailID)
            {
                target = trail.transform.GetChild(trailCount);
                currentPath = trail;
                break;
            }
            trailCount++;
        }
        if(trailID == -1)
        {
            print("Error: Couldn't find trailID!");
            target = tempTrail[0].spawnPoint;
            currentPath = tempTrail[0];
        }

        //target = Waypoints.points[0];

    }

    // Update is called once per frame
    void Update ()
	{
		
		Vector3 dir = target.position - transform.position;
		transform.Translate (dir.normalized * enemy.speed * Time.deltaTime, Space.World);
		transform.LookAt(target);

		if (Vector3.Distance (transform.position, target.position) <= 0.2f) {
			GetNextWaypoint();
			print(target);
		}
	}

	void GetNextWaypoint()
	{
		if (wavepointIndex >= currentPath.points.Length - 1)
		{
			EndPath();
			return;
		}

		wavepointIndex++;
		target = currentPath.points[wavepointIndex];
	}

    public int SetTrailID()
    {
        return trailID;
    }

    public void SetTrailID(int trail)
    {
        trailID = trail;
    }

	void EndPath()
	{
//		//PlayerStats.Lives--;
//		DamagePlayer(
		WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
	}

//	void DamagePlayer (Transform enemy)
//	{
//		PlayerStats p = enemy.GetComponent<Enemy>();
//
//		p.TakeDamage(damage);
//	}
}