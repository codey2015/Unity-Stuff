using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {
    public bool startPaused = true;
    public GameObject placeTowersDialogue;
    public float towerSetUpTime = 10f;
    [HideInInspector]
    public GameObject towerTutorialDialogue;
    [HideInInspector]
    public float towerTutorialUnpauseafter = 5f;
    public GameObject towerTypeTutorialDialogue;
    public float towerTypeUnpauseafter = 5f;
    public GameObject ammoStationTutorialDialogue;
    public float ammoStationUnpauseafter = 5f;
    public GameObject enemyTypeTutorialDialogue;
    public float enemyTypeUnpauseafter = 5f;
    public GameObject upgradeStationTutorialDialogue;
    public float upgradeStationUnpauseafter = 5f;

    void Start () {
        //start the game paused so things don't spawn
        StartCoroutine(setUp());

        if(startPaused == true)
        {
            Time.timeScale = 0;
        }
        print(Time.timeScale + "TIMESCALE");
        //StartCoroutine(TowerSetUp());
                //StartCoroutine(TowerTutorial());
        //StartCoroutine(TowerTypes());
        //StartCoroutine(AmmoStationSetUp());
        //StartCoroutine(EnemyType());
        //StartCoroutine(UpgradeStation());

    }


    IEnumerator TowerSetUp()
    {
        //dialog activated, player gets a time to set up towers
        placeTowersDialogue.SetActive(true);
        yield return new WaitForSeconds(towerSetUpTime);
        placeTowersDialogue.SetActive(false);
        StartCoroutine(TowerTypes());

    }

    IEnumerator TowerTutorial()
    {
        towerTutorialDialogue.SetActive(true);
        yield return new WaitForSeconds(towerTutorialUnpauseafter);
        towerTutorialDialogue.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator TowerTypes()
    {
        //Time.timeScale = 0;
        towerTypeTutorialDialogue.SetActive(true);
        yield return new WaitForSeconds(towerTypeUnpauseafter);
        towerTypeTutorialDialogue.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator AmmoStationSetUp()
    {
        //Time.timeScale = 0;
        ammoStationTutorialDialogue.SetActive(true);
        yield return new WaitForSeconds(ammoStationUnpauseafter);
        ammoStationTutorialDialogue.SetActive(false);
        StartCoroutine(AmmoStationSetUp());
        Time.timeScale = 1;
    }

    IEnumerator EnemyType()
    {
        //Time.timeScale = 0;
        enemyTypeTutorialDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(enemyTypeUnpauseafter);
        enemyTypeTutorialDialogue.SetActive(false);
        StartCoroutine(EnemyType());
        Time.timeScale = 1;
    }

    IEnumerator UpgradeStation()
    {
        //Time.timeScale = 0;
        upgradeStationTutorialDialogue.SetActive(true);
        yield return new WaitForSeconds(upgradeStationUnpauseafter);
        upgradeStationTutorialDialogue.SetActive(false);
        StartCoroutine(UpgradeStation());
        Time.timeScale = 1;
    }


    IEnumerator setUp()
    {
        placeTowersDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(towerSetUpTime);
        placeTowersDialogue.SetActive(false);

        towerTypeTutorialDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(towerTypeUnpauseafter);
        towerTypeTutorialDialogue.SetActive(false);

        ammoStationTutorialDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(ammoStationUnpauseafter);
        ammoStationTutorialDialogue.SetActive(false);

        enemyTypeTutorialDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(enemyTypeUnpauseafter);
        enemyTypeTutorialDialogue.SetActive(false);

        upgradeStationTutorialDialogue.SetActive(true);
        yield return new WaitForSecondsRealtime(upgradeStationUnpauseafter);
        upgradeStationTutorialDialogue.SetActive(false);
    }
}
