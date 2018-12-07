using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnDeaths : MonoBehaviour {
    private List<Transform> enemy;
    private Enemy deadEnemy;
    public static int count;
    public static int totalEnemies;
    public int currentDeathAmount;
    [Header("Based on number of enemy deaths,")]
    [Header("spawn a group of new enemies.")]
    public int numberOfEnemyDeaths = 10;

    public AudioSource[] alienDeathSounds;
    private AudioSource playDeathSound;


    [Range(0, 0.5f)]
    public float pitchRandomize;
    float originalPitch;

    [HideInInspector]
    public int totalNumberOfEnemies;
    private void Start()
    {
        countEnemies();
        print(totalEnemies + ": Total Enemies");
        totalNumberOfEnemies = totalEnemies;

        if (playDeathSound == null)
        {
            playDeathSound = gameObject.GetComponent<AudioSource>();
            playDeathSound.Stop();
            originalPitch = playDeathSound.pitch;
        }
    }

    void Update () {

        //count = Enemy.count;
        if(Enemy.countOne == true)
        {
            playDeathSound = alienDeathSounds[Random.Range(0, alienDeathSounds.Length)];
            playDeathSound.Play();
            if (playDeathSound != null)
            {
                float newPitch = Random.Range(-pitchRandomize, pitchRandomize) + playDeathSound.pitch;
                playDeathSound.pitch = newPitch;
                playDeathSound.Play();
                StartCoroutine(pitchShift());
            }
            Bobber.countFootStepNumber--;
            count++;
            currentDeathAmount = enemySpawnDeaths.count;
            Enemy.countOne = false;
        }

        if(count == numberOfEnemyDeaths)
        {
            spawnGroup();
        }

	}

    void spawnEnemy(Transform child)
    {
        child.gameObject.SetActive(true);
    }


    void spawnGroup()
    {
        foreach (Transform child in transform)
        {
            spawnEnemy(child);
        }
    }
    
    void countEnemies()
    {
        foreach (Transform child in transform)
        {
            totalEnemies += 1;
        }
     }

    IEnumerator pitchShift()
    {
        yield return new WaitForSeconds(.5f);
        playDeathSound.pitch = originalPitch;
    }
}
