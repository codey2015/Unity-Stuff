using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class RespawnKey : MonoBehaviour {

    public Vector3 spawnPoint;
    public Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
        spawnPoint = this.transform.position;

    }
	
	void FixedUpdate () {

        if (Invector.CharacterController.vThirdPersonController.instance.currentHealth != Invector.CharacterController.vThirdPersonController.instance.maxHealth)
        {
            this.transform.position = spawnPoint;
            print("DEAD");
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "movingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "movingPlatform")
        {
            transform.parent = null;

        }
    }
}
