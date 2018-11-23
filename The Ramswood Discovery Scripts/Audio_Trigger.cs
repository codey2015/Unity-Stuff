using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Audio_Trigger : MonoBehaviour
{
    public bool activateTrigger = false;

    public Image textO;
    public AudioSource Sound;
    public int count = 0;
    public float volume;
    private bool playOnce = true;
    void Start()
    {
        textO.enabled = (false);
        volume = Sound.volume;
    }

    void Update()
    {
        if (activateTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (playOnce == true)
            {
                Sound.Play();
            }
            Sound.volume = volume;
            count += 1;
        }
        if (count >= 2)
        {
            Sound.volume = 0;
            count = 0;
            playOnce = false;
        }
        if (Time.timeScale == 0)
        {
            textO.enabled = (false);
        }

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            textO.enabled = (true);
            activateTrigger = true;
        }
    }


    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            textO.enabled = (false);
            activateTrigger = false;
        }
    }
}
