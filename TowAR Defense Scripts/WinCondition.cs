using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour {

    public GameObject winScreen;

    public int deathCount;
    public int totalEnemies;
	void Update () {

        deathCount = enemySpawnDeaths.count;
        totalEnemies = enemySpawnDeaths.totalEnemies;
        if (enemySpawnDeaths.count >= enemySpawnDeaths.totalEnemies)
        {
            winScreen.SetActive(true);
        }

	}
}
