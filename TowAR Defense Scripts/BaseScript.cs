using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour {

    public GameObject EndCanvas;

    [Header("Attach Kinematic Rigidbody and isTrigger BoxCollider")]

    public float startHealth = 500;

    [SerializeField]
    private float health;

    public ParticleSystem attachetParticleSystem;
    public float playParticlesForSeconds = 1f;


    public AudioSource impactSound;
    [Range(0, 0.5f)]
    public float pitchRandomize;
    float originalPitch;

    [Header("Add this audio source to child")]
    public AudioSource deathSound;

    [HideInInspector]
    public Image healthBarSide;
    [HideInInspector]
    public Image healthBarTop;

    void Start()
    {
        health = startHealth;
        if (attachetParticleSystem == null)
        {
            attachetParticleSystem = GetComponent<ParticleSystem>();
        }
        if (attachetParticleSystem != null)
        {
            attachetParticleSystem.Stop();
        }

        if (impactSound == null)
        {
            impactSound = gameObject.GetComponent<AudioSource>();
            impactSound.Stop();
            originalPitch = impactSound.pitch;
        }
        if(impactSound!= null)
        {
            originalPitch = impactSound.pitch;
        }
        if(deathSound == null)
        {
            deathSound = gameObject.transform.GetChild(0).GetComponentInChildren<AudioSource>();
            deathSound.Stop();
        }
    }


   void TakeDamage(float damageDealt)
    {
        health -= damageDealt;
    }

    void DestroyBase()
    {
        if (health <= 0)
        {
            if (deathSound != null)
            {
                deathSound.Play();
            }
            EndCanvas.SetActive(true);
            Time.timeScale = 0;
            //Destroy(gameObject);
        }
    }
     
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            Enemy thisEnemy = other.gameObject.GetComponent<Enemy>();
            //enemySpawnDeaths.count++;
            //Bobber.countFootStepNumber--;

            if (other.gameObject.activeSelf)
            {
                health -= thisEnemy.towerDamage;
                if (attachetParticleSystem != null)
                {
                    StartCoroutine(playParticles());
                }
                if (impactSound != null)
                {
                    float newPitch = Random.Range(-pitchRandomize, pitchRandomize) + impactSound.pitch;
                    impactSound.pitch = newPitch;
                    impactSound.Play();
                    StartCoroutine(pitchShift());
                }
                thisEnemy.health = -1;
                thisEnemy.TakeDamage(100000);
                enemySpawnDeaths.count--;
                Bobber.countFootStepNumber--;
            }
            //Destroy(other.gameObject);
            DestroyBase();
        }



    }
    IEnumerator playParticles()
    {
        attachetParticleSystem.Play();
        yield return new WaitForSeconds(playParticlesForSeconds);
        attachetParticleSystem.Stop();
    }


    IEnumerator pitchShift()
    {
        yield return new WaitForSeconds(.5f);
        impactSound.pitch = originalPitch;
    }
}
