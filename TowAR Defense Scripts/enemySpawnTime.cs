using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnTime : MonoBehaviour {


    public List<Transform> enemy;

    [Header("Put this script on a parent group.\n")]
    [Header(" It spawns all deactivated children\n")]
    [Header(" at the specified time.")]
    [Space(10)]
    public float firstSpawnTime = 3f;
    public float spawnIncrementTime = 1.5f;

    private int pos = 0;
    private int count;
    void Start () {

        foreach (Transform child in transform)
        {
            enemy.Add(child);
            enemySpawnDeaths.totalEnemies += 1;
            count += 1;
        }
       

        if (count > 0)
        {
            InvokeRepeating("SpawnEnemy", firstSpawnTime, spawnIncrementTime);
        }

    }

    void SpawnEnemy()
    {
        if (count > 0)
        {
            enemy[pos].gameObject.SetActive(true);
            pos += 1;
            count -= 1;
        }
    }
}
