using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class endingScript : MonoBehaviour
{


    public Transform[] goTo;
    public FirstPersonController me; //player controller
    public Camera cam;
    public float speed = 1;
    private int nextPoint = 0;
    private bool wentIn = false;
    private bool playOnce = false;
    private bool playOnceEnd = true;
    public AudioSource truck;
    public AudioSource endingMusic;

    public objectivesFinished OF;
    public pause PM;
    public Image credits;
    // Use this for initialization
    void Start()
    {
        me = GameObject.FindObjectOfType<FirstPersonController>();
        OF = GameObject.FindGameObjectWithTag("Canvas").GetComponent<objectivesFinished>();
        PM = GameObject.FindGameObjectWithTag("Canvas").GetComponent<pause>();
        credits.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (wentIn == true && playOnce == true)
        {

            Time.timeScale = 0;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, goTo[nextPoint].position, speed);
            rotToward();

            if (Vector3.Distance(cam.transform.position, goTo[nextPoint].position) < 1.5f)
            {
                GotoNextPoint();
                Debug.Log("Reached the next Point");
                if (playOnceEnd == true)
                {
                    endingMusic.time = 1;
                    endingMusic.Play();
                    credits.enabled = true;
                    playOnceEnd = false;
                }
            }
        }
        

    }

    void rotToward()
    {
        Vector3 targetDir = goTo[nextPoint].up - cam.transform.position;
        Vector3 newDir = Vector3.RotateTowards(cam.transform.forward, targetDir, speed / 75, 0.0f);
        cam.transform.rotation = Quaternion.LookRotation(newDir);

    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (goTo.Length == 0)
            return;

        nextPoint = (nextPoint + 1) % goTo.Length;

    }


    IEnumerator TruckStarting()
    {
        truck.time = 3;
        truck.Play();
        yield return new WaitForSeconds(4);
        playOnce = true;
        
    }


    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" && OF.getOut == true)
        {
            StartCoroutine(TruckStarting());
            wentIn = true;
            me.enabled = false;
            PM.checkUI = true;
        }

    }

}
