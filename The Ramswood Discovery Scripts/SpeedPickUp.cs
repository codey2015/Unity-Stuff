using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SpeedPickUp : MonoBehaviour {


    public float speedPowerUp = 3;
    public int powerUpTime = 4;
    Renderer rend;
    Collider m_Collider;
    public FirstPersonController me;

    void Start () 
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        m_Collider = GetComponent<Collider>();
        me = GameObject.FindObjectOfType<FirstPersonController>();
    }
 
    IEnumerator Wait()
    {
        rend.enabled = false;
        m_Collider.enabled = false;
        me.timer = true;
        me.speed += speedPowerUp;
        yield return new WaitForSeconds(powerUpTime);
        me.check = true;
        Destroy(gameObject);
        //Debug.Log("hmmmmmm");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Wait());
        }
    }
}
