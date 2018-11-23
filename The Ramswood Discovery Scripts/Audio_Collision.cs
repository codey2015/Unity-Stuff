using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Collision : MonoBehaviour
{
    public AudioSource audioSource;
    private bool playOnce = false;
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        //foreach (ContactPoint contact in collision.contacts)
        //{
        //Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}

        //if (collision.relativeVelocity.magnitude > 2)
        if (other.gameObject.tag == "Player")
        {
            if (playOnce == false)
            {
                audioSource.Play();
                playOnce = true;
            }
        }
    }
}