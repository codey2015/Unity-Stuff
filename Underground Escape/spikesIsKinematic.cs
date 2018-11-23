using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikesIsKinematic : MonoBehaviour {
    public Rigidbody rb;
    public Vector3 spawnPoint;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        spawnPoint = this.transform.position;

    }

    void FixedUpdate()
    {

        if (Invector.CharacterController.vThirdPersonController.instance.currentHealth != Invector.CharacterController.vThirdPersonController.instance.maxHealth)
        {
            this.transform.position = spawnPoint;
            rb.isKinematic = true;

        }
    }
}
