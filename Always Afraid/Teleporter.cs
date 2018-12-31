using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Teleporter : MonoBehaviour {

    public GameObject obj1;
    public GameObject obj2;

    public bool teleportFromObj1 = true;
    public bool teleportFromObj2 = true;

    private bool exitedObj1 = true;
    private bool exitedObj2 = true;

    private Transform objectToTeleport;

    void Start () {
        objectToTeleport = gameObject.transform;       
	}

    private void OnTriggerEnter(Collider other)
    {    
        if (other == obj1.GetComponent<Collider>() && teleportFromObj1 && exitedObj1 == true)
        {
            exitedObj2 = false;
            objectToTeleport.position = obj2.transform.position;
        }
        if (other == obj2.GetComponent<Collider>() && teleportFromObj2 && exitedObj2 == true)
        {
            exitedObj1 = false;
            objectToTeleport.position = obj1.transform.position;
        }      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == obj1.GetComponent<Collider>())
        {
            exitedObj1 = true;
        }
        if (other == obj2.GetComponent<Collider>())
        {
            exitedObj2 = true;
        }
     }

}
