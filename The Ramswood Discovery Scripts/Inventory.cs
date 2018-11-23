using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Inventory : MonoBehaviour
{
    public float speedPowerUp = 3;
    public int powerUpTime = 4;
    Renderer rend;
    Collider col;
    public FirstPersonController me;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        col = GetComponent<Collider>();
        col.enabled = true;
        me = GameObject.FindObjectOfType<FirstPersonController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            col.enabled = false;
            rend.enabled = false;
            me.inventory += 1;
        }
    }
}

