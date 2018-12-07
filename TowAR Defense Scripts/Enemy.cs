using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    [HideInInspector]
	public float startSpeed = 2f;
	public float startHealth = 100f;

    //[HideInInspector]
    [Tooltip("Shows current health.")]
    public float health;

    [HideInInspector]
    public static bool countOne = false;
    [HideInInspector]
    public float damagedBy = 1;

    //[SerializeField]
    private int playedXTimes = 0;

    [HideInInspector]
    public float speed;
    public float towerDamage = 10f;
    public int deathXP;

    public Color changeColorTo = Color.red;
    public float changeColorSpeed = 5f;
    public float playColorFor = .5f;
    public bool simplifyColorChange = false;

    public GameObject flashObject;
    public float flashTime = .05f;

    [Tooltip("For testing purposes.")]
    public Color myColor;
    private Color startColor;
    private float t = 0f;

    //[SerializeField]
    private bool activateColorChange = false;
    private GameObject statTracker;
    

    public enum colorOptions
    {
        Red,
        Blue,
        Green
    };
    [SerializeField]
    public colorOptions EnemyColor;

    
    void Start ()
	{
		speed = startSpeed;
		health = startHealth;

        // Probably need to optimize how the enemy script references the StatTracker
        statTracker = GameObject.FindGameObjectWithTag("StatTracker");
        Color color = GetComponent<Renderer>().material.color;
        startColor = color;
        if (flashObject != null)
        {
            flashObject.SetActive(false);
        }
    }
    void Update()
    {
        if (activateColorChange == true)
        {
            if (simplifyColorChange == false)
            {
                changeColor();
            }

            if(simplifyColorChange == true)
            {
                changeColorSimplified();
            }
        }

    }

    public void TakeDamage (float damageDealt)
	{
		health -= damageDealt;
		if (health <= 0) 
		{
            AudioSource sound = GetComponent<AudioSource>();
            if (sound.isActiveAndEnabled && sound!=null)
            {
                Bobber.countFootStepNumber--;
            }
            //Bobber.countFootStepNumber = 3;
            countOne = true;
            Die();
            //Add death XP
		}

        playedXTimes += 1;

        //StartCoroutine(waitColor());
        float elapsed = 0;
        elapsed += Time.deltaTime;

        if (flashObject != null)
        {
            StartCoroutine(flashColor());
        }



        activateColorChange = true;
        if(elapsed > 1f)
        {
            activateColorChange = false;
        }
        
    }


    void changeColor()
    {
        Color color = Color.Lerp(startColor, changeColorTo, t);
        t += Time.deltaTime / changeColorSpeed;
        myColor = color;
        GetComponent<Renderer>().material.SetColor("_Color", color);

    }

    void changeColorSimplified()
    {
        playColorFor = 2f;
        //playColorFor = 1f;
        changeColorSpeed = startHealth / (damagedBy * playColorFor);
        Color color = Color.Lerp(startColor, changeColorTo, t);
        t += Time.deltaTime / changeColorSpeed;
        myColor = color;
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }
    public void SlowEnemy (float percent)
	{
		speed = startSpeed * (1f - percent);
	}

	void Die() 
	{
        if(statTracker!=null)
        statTracker.GetComponent<StatTracking>().AddXP(deathXP);
        enemySpawnDeaths.count += 1;
		Destroy(gameObject);
	}

    public IEnumerator waitColor()
    {
        activateColorChange = true;
        yield return new WaitForSeconds(playColorFor);
        activateColorChange = false;
        playedXTimes -= 1;
    }

    public IEnumerator flashColor()
    {
        GetComponent<MeshRenderer>().enabled = false;
        flashObject.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        GetComponent<MeshRenderer>().enabled = true;
        flashObject.SetActive(false);
    }

}