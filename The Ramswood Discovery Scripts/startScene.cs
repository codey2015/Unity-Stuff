using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class startScene : MonoBehaviour {

    public FirstPersonController me; //player controller
    private bool startGame = false;
    public GameObject boss;
    private pause PM;
    public AudioSource sound;
    public Camera cam;
    public Transform look;
    // Use this for initialization
    void Start () {
        me = GameObject.FindObjectOfType<FirstPersonController>();
        PM = GameObject.FindGameObjectWithTag("Canvas").GetComponent<pause>();

        boss.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator waitForBoss()
    {
        me.enabled = false;
        boss.SetActive(true);
        PM.checkUI = true;
        sound.Play();
        cam.transform.LookAt(look.transform.position);
        yield return new WaitForSeconds(5.5f);
        me.enabled = true;
        boss.SetActive(false);
        PM.checkUI = false;
        sound.Stop();
        startGame = true;

    }
    void rotToward()
    {
        Vector3 targetDir = look.transform.forward - cam.transform.position;
        Vector3 newDir = Vector3.RotateTowards(cam.transform.forward, targetDir, 5 / 75, 0.0f);
        cam.transform.rotation = Quaternion.LookRotation(newDir);

    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && startGame == false)
        {

            StartCoroutine(waitForBoss());
        }

    }
}
