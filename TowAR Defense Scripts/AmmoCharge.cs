using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCharge : MonoBehaviour {

    //private Cannon cannon;
    //private Enemy enemy;
    public float superEffectiveDamage = 15f;
    public float effectiveDamage = 10f;
    public float notVeryEffectiveDamage = 5f;

    public float ammoRunOutTime = 10f;
    public float ammoWaitTime = 5f;

    public static float add;
    public static float sub;
    public static float med;
    //[HideInInspector]
    //public static bool strongerAmmo = false;
    public static bool compareOnce = false;

    public Color ActivatedColor;
    public Color doneWaitingColor;

    private float elapsed = 0f;
    private Color startColor;
    private bool checkCharged = false;
    private bool checkCharging = false;
    private bool getOff = false;

    [HideInInspector]
    public bool isRED = false;
    [HideInInspector]
    public bool isBLUE = false;
    [HideInInspector]
    public bool isGREEN = false;

    [HideInInspector]
    public towerAttack t;
    [Header("Attatch 3 AudioSource by order")]
    public AudioSource chargeStation;
    public AudioSource doneCharging;
    public AudioSource enteredStation;
    public AudioSource cancelCharge;

    public enum colorOptions
    {
        Red,
        Blue,
        Green
    };
    [SerializeField]
    public colorOptions stationColor;


    void Start () {
        Color color = GetComponent<Renderer>().material.color;
        startColor = color;

        if (chargeStation == null)
        {
            chargeStation = gameObject.transform.GetChild(0).GetComponentInChildren<AudioSource>();
            chargeStation.Stop();
        }
        if (doneCharging == null)
        {
            doneCharging = gameObject.transform.GetChild(1).GetComponentInChildren<AudioSource>();
            doneCharging.Stop();
        }
        if (enteredStation == null)
        {
            enteredStation = gameObject.transform.GetChild(2).GetComponentInChildren<AudioSource>();
            enteredStation.Stop();
        }
        if (cancelCharge == null)
        {
            cancelCharge = gameObject.transform.GetChild(3).GetComponentInChildren<AudioSource>();
            cancelCharge.Stop();
        }

    }

    IEnumerator playOffSound()
    {
        cancelCharge.Play();
        print("cancelCharge.Play();");
        yield return new WaitForSeconds(1f);
        getOff = false;
    }


    IEnumerator ammoForTime()
     {
        yield return new WaitForSeconds(ammoRunOutTime);
        t.strongerAmmo = false;
        t.particleChargeRED.SetActive(false);
        t.particleChargeBLUE.SetActive(false);
        t.particleChargeGREEN.SetActive(false);
        print("Out of Ammo");
        
    }

    IEnumerator ammoForWaitTime()
    {
        add = superEffectiveDamage - 10f;
        sub = notVeryEffectiveDamage;
        med = effectiveDamage;
        yield return new WaitForSeconds(0);
        print("Ammo Upgraded");
        t.strongerAmmo = true;
        //compareOnce = true;
    }


    void OnTriggerEnter(Collider other)
    {

        //Must have rigidbody attached(make sure to check isKinematic)
        if (other.gameObject.tag == "Tower")
        {
            enteredStation.Play();
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        //Must have rigidbody attached(make sure to check isKinematic)
        if (other.gameObject.tag == "Tower")
        {
            t = other.GetComponent<towerAttack>();
            if (checkCharging == false)
            {
                chargeStation.Play();
                checkCharging = true;
            }
            //doneCharging.Play();
            switch (stationColor)
            {
                case AmmoCharge.colorOptions.Red:
                    t.TowerColor = towerAttack.colorOptions.Red;
                    isRED = true;
                    break;
                case AmmoCharge.colorOptions.Blue:
                    t.TowerColor = towerAttack.colorOptions.Blue;
                    isBLUE = true;
                    break;
                case AmmoCharge.colorOptions.Green:
                    t.TowerColor = towerAttack.colorOptions.Green;
                    isGREEN = true;
                    break;
            }
            Color color = GetComponent<Renderer>().material.color;
            color = ActivatedColor;
            GetComponent<Renderer>().material.SetColor("_Color", color);

            elapsed += Time.deltaTime;
            if (elapsed >= ammoWaitTime)
            {
                if(checkCharged == false)
                {
                    doneCharging.Play();
                    checkCharged = true;
                }
                chargeStation.Stop();
                
                if (isRED == true)
                {
                    t.particleChargeRED.SetActive(true);
                }
                if(isBLUE == true)
                {
                    t.particleChargeBLUE.SetActive(true);
                }
                if (isGREEN == true)
                {
                    t.particleChargeGREEN.SetActive(true);
                }

                color = doneWaitingColor;
                GetComponent<Renderer>().material.SetColor("_Color", color);

                compareOnce = false;
                print("Tower Entered");
                StartCoroutine(ammoForWaitTime());
            }
        }
     }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            chargeStation.Stop();
            print("Tower Exited");
            StartCoroutine(ammoForTime());
            elapsed = 0f;
            if(checkCharged == false)
            {
                getOff = true;
            }

            compareOnce = true;
            checkCharged = false;
            checkCharging = false;
            Color color = GetComponent<Renderer>().material.color;
            color = startColor;
            GetComponent<Renderer>().material.SetColor("_Color", color);
            if (getOff == true)
            {
                StartCoroutine(playOffSound());
            }
        }
    }

}
