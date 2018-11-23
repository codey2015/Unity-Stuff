using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BearTrap : MonoBehaviour
{


    public float speedPowerDownRun = 10;
    public float speedPowerDownWalk = 4;
    public int powerDownTime = 4;
    Renderer rend2;
    //Collider m_Collider2;
    public FirstPersonController me2;
    public AudioSource sound;

    void Start()
    {
        rend2 = GetComponent<Renderer>();
        rend2.enabled = true;
        //m_Collider2 = GetComponent<Collider>();
        me2 = GameObject.FindObjectOfType<FirstPersonController>();

    }

    void Update()
    {
    }

    IEnumerator Wait2()
    {
        //rend2.enabled = false;
        //m_Collider2.enabled = false;
        sound.Play();
        me2.timer = true;
        me2.trap = true;
        if (me2.isWalking == true)
        {
            me2.speed -= speedPowerDownWalk;
            me2.stamina = 0;
            Debug.Log("isWalking == true");
        }
        if (me2.isWalking == false)
        {
            me2.speed -= speedPowerDownRun;////////
            me2.stamina = 0;
            Debug.Log("isWalking == false");

        }
        yield return new WaitForSeconds(powerDownTime);
        if (me2.isWalking == true)
        {
            me2.speed += speedPowerDownWalk;
        }
        if (me2.isWalking == false)
        {
            me2.speed += speedPowerDownRun;////////
        }

        me2.check = true;
        //Destroy(gameObject);
        //Debug.Log("hmmmmmm");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("WORKIddddddddddddddNG");
            StartCoroutine(Wait2());
            //StopCoroutine("Wait");
        }
    }

}
