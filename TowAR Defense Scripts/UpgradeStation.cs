using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{

    public GameObject statTracker;

    public float ammoWaitTime = 3f;

    public static bool compareOnce = false;

    public Color activatedColor;
    private Color normalColor;

    private float elapsed = 0f;



    void Start()
    {
        statTracker = GameObject.FindGameObjectWithTag("StatTracker");
        Color color = GetComponent<Renderer>().material.color;
        normalColor = color;
    }



    void OnTriggerStay(Collider other)
    {

        //Must have rigidbody attached(make sure to check isKinematic)
        if (other.gameObject.tag == "Tower")
        {
            GetComponent<Renderer>().material.color = activatedColor;

            elapsed += Time.deltaTime;
            //print(elapsed);
            if (elapsed >= ammoWaitTime)
            {
                //GetComponent<Renderer>().material.color = normalColor;

                print("Tower Entered");

                // necessary to call these before swapping out the tower
                this.gameObject.SetActive(false);
                statTracker.GetComponent<StatTracking>().SetNextUpgrade();
                elapsed = -100000f;

                other.gameObject.GetComponent<towerAttack>().UpgradeTower();

            }
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            GetComponent<Renderer>().material.color = normalColor;
            print("Tower Exited");
            elapsed = 0f;            
        }
    }
    
}
