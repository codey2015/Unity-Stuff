using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class towerAttack : MonoBehaviour {

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Enemy targetEnemy;
    [HideInInspector]
    public float speed = 20f;
    [HideInInspector]
    public float turnSpeed = 3f;
    [HideInInspector]
    public string enemyTag = "Enemy";
    [HideInInspector]
    public bool strongerAmmo = false;

    public Transform ship;
    public Transform firePoint;
    public GameObject cannonBallPrefab;
    public GameObject nextUpgradePrefab;
    public GameObject impactEffect;
    public GameObject particleChargeRED;
    public GameObject particleChargeBLUE;
    public GameObject particleChargeGREEN;

    public float fineTuneSpeed = 1.05f;
    public float maxDistanceMultiplier = 1.5f;
    public float hurtAt = .3f;
    public float range = 10f;
    public float fireRate = 1f;
    public float lookForEnemyEvery = .75f;
    public float damage = 50f;
    public float destroyCannonBallTime = 1.5f;
    public float blastRadius = 0f;
    public float destroyEffectTime = 1.5f;
    public float fallOffDamage = 5f;
    public float autoSetDamageMultiplier = .5f;
    public float[] fallOffDamageMultiplier = new float[9];

   
    public Material[] towerMaterials;
    public int matIndex = 0;
    public AudioSource blaster;
    [Range(0,0.5f)]
    public float pitchRandomize;


    protected float fireCountDown = 0f;
    protected float TempDmg;
    protected bool newTarget = false;
    protected Vector3 dir;
    protected float distanceThisFrame;
    float originalPitch;
    protected GameObject cannonBallGO;
    protected GameObject effectInst;

    //############ Used to provide the user a choice of color for the towers ############//
    public enum colorOptions
    {
        Red,
        Blue,
        Green
    };
    public colorOptions TowerColor;

    //CannonBall2 cannonBall;
    AmmoCharge ammo;

    //############ Allows the designer to see the radius of affect ############//



    //############ Does functions on start ############//
    protected virtual void Start()
    {

        //repeat every .75 seconds
        InvokeRepeating("UpdateTarget", 0f, lookForEnemyEvery);
        ammo = this.GetComponent<AmmoCharge>();
        TempDmg = damage;

        ship = gameObject.transform;

        float tempSetMultiplier = autoSetDamageMultiplier;

        for ( int i = 0; i< fallOffDamageMultiplier.Length;i++)
        {
            if (fallOffDamageMultiplier[i] == 0)
            {
                fallOffDamageMultiplier[i] = tempSetMultiplier;
            }
            tempSetMultiplier += autoSetDamageMultiplier;
        }


        if (blaster == null)
        {
            blaster = gameObject.GetComponent<AudioSource>();
            blaster.Stop();
            originalPitch = blaster.pitch;
        }
        GetComponentInChildren<Renderer>().material = towerMaterials[matIndex];

    }

    protected virtual void Update()
    {
        if (target == null)
        {
            UpdateTarget();
            return;
        }

        dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(ship.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        ship.LookAt(target);

        if (fireCountDown <= 0)
        {
            if (CanSeeEnemy(target.gameObject))
            {
                Shoot();
            }
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;

        distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        if(strongerAmmo == false)
        {
            
            damage = TempDmg;

            //particleChargeRED.SetActive(false);
            //particleChargeBLUE.SetActive(false);
            //particleChargeGREEN.SetActive(false);
            //AmmoCharge.isRED = false;
            //AmmoCharge.isBLUE = false;
            //AmmoCharge.isGREEN = false;


        }

        if (cannonBallGO != null)
        {
            Seek(target);
            cannonBallGO.transform.LookAt(target);
            Vector3 vec = (firePoint.transform.position - targetEnemy.transform.position) * maxDistanceMultiplier;
            cannonBallGO.transform.Translate(-vec * Time.deltaTime * maxDistanceMultiplier * fineTuneSpeed, Space.World);

            float distanceToEnemy2 = Vector3.Distance(cannonBallGO.transform.position, targetEnemy.transform.position);
            if (distanceToEnemy2 < hurtAt)
            {
                Destroy(cannonBallGO);
                if (blastRadius > 0f)
                {
                    effectInst = (GameObject)Instantiate(impactEffect, cannonBallGO.transform.position, cannonBallGO.transform.rotation);
                    Destroy(effectInst, destroyEffectTime);
                    Explode();
                }
                else
                {
                    Damage(targetEnemy.transform);

                }

            }
            else
            {
                StartCoroutine(destroyCannonBall(cannonBallGO));
            }
            StartCoroutine(pitchShift());
        }
    }
    
    protected IEnumerator destroyCannonBall(GameObject g)
    {
        yield return new WaitForSeconds(destroyCannonBallTime);
        Destroy(g);
    }


    protected IEnumerator pitchShift()
    {
        yield return new WaitForSeconds(.5f);
        blaster.pitch = originalPitch;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.blue;
        if(effectInst!=null)
        Gizmos.DrawWireSphere(effectInst.transform.position, blastRadius);
        
    }

    //############ Updates the tower to find the nearest enemy ############//
    protected virtual void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;

        float shortestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
                if (nearestEnemy)
                {
                    newTarget = true;
                    print("NEW TARGET");
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            newTarget = true;
            print("NEW TARGET");
        }
        else
        {
            target = null;
        }
        if(target!=null)
            targetEnemy = target.GetComponent<Enemy>();
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }
    //############ Shoot funtion used for launching projectiles ############//

    protected void Shoot()
    {
        cannonBallGO = (GameObject)Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        if (blaster != null)
        {
            float newPitch = Random.Range(-pitchRandomize, pitchRandomize) + blaster.pitch;
            blaster.pitch = newPitch;
            blaster.Play();
        }
        
        if (cannonBallGO != null)
        {
            if (strongerAmmo == true && AmmoCharge.compareOnce == true)
            {
                compareColors();
                if (newTarget == true)
                {
                    damage = TempDmg;
                    compareColors();
                    newTarget = false;
                    AmmoCharge.compareOnce = true;
                }
            }
        }
    }

    protected bool CanSeeEnemy(GameObject target)
    {
        RaycastHit info;
        Debug.DrawLine(firePoint.transform.position, targetEnemy.transform.position);
        if (Physics.Raycast(firePoint.transform.position, targetEnemy.transform.position - firePoint.transform.position, out info, range))
        {
            Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.cyan);
            if (info.collider.tag == "Enemy")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void UpgradeTower()
    {
        GameObject upgradedTower = Instantiate(nextUpgradePrefab, gameObject.transform.position, gameObject.transform.rotation);
        // parent to card
        upgradedTower.transform.SetParent(gameObject.transform.parent.transform, true);
        upgradedTower.GetComponent<towerAttack>().towerMaterials = towerMaterials;
        upgradedTower.GetComponent<towerAttack>().matIndex = ++matIndex;
        Destroy(gameObject);
    }


    protected void HitTarget()
    {
        GameObject effectInst = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInst, 3f);

        if (blastRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);

    }

    protected virtual void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(cannonBallGO.transform.position, blastRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                 float distanceBetween = Vector3.Distance(cannonBallGO.transform.position, collider.transform.position);
                //if blast radius == 12: If enemy position and explosion position is less than 1.2r away, do regular damage 
                if (distanceBetween < blastRadius / 10)
                {
                    Damage(collider.transform);
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 1.3r away
                if (distanceBetween < blastRadius / 9)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[0]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 1.5r away
                if (distanceBetween < blastRadius / 8)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[1]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 1.7r away
                if (distanceBetween < blastRadius / 7)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[2]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 2r away
                if (distanceBetween < blastRadius / 6)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[3]));
                    continue;
                }

                //if blast radius == 12: If enemy position and explosion position is less than 2.4r away
                if (distanceBetween < blastRadius / 5)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[4]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 3r away
                if (distanceBetween < blastRadius / 4)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[5]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 4r away
                if (distanceBetween < blastRadius / 3)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[6]));
                    continue;
                }
                //if blast radius == 12: If enemy position and explosion position is less than 6r away
                if (distanceBetween < blastRadius / 2)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[7]));
                    continue;

                }
                //if blast radius == 12: If enemy position and explosion position is less than 12r away
                if (distanceBetween < blastRadius / 1)
                {
                    Enemy e = collider.GetComponent<Enemy>();
                    e.TakeDamage(damage - (fallOffDamage + fallOffDamageMultiplier[8]));
                    continue;

                }
            }
        }
    }

    protected virtual void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.damagedBy = damage;
            e.TakeDamage(damage);
        }
    }


    //############ Used to compare the colors of the enemy and the tower ############//

    void compareColors()
    {
        print("TowerColor: " + TowerColor);
        print("EnemyColor: " + targetEnemy.EnemyColor);
        if (TowerColor == colorOptions.Red)
        {
            print("TowerColor == colorOptions.Red");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Green)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Green");
                damage += AmmoCharge.add;
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Blue)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Blue");
                damage += AmmoCharge.sub;
            }
            else
            {
                damage += AmmoCharge.med;
            }
        }

        if (TowerColor == colorOptions.Blue)
        {
            print("TowerColor == colorOptions.Blue");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Red)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Red");
                damage += AmmoCharge.add;
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Green)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Green");
                damage += AmmoCharge.sub;
            }
            else
            {
                damage += AmmoCharge.med;
            }
        }

        if (TowerColor == colorOptions.Green)
        {
            print("TowerColor == colorOptions.Green");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Blue)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Blue");
                print("DAMAGE: " + damage);
                damage += AmmoCharge.add;
                print("DAMAGE2: " + AmmoCharge.add);
                print("New Damage: " + damage);
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Red)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Red");
                damage += AmmoCharge.sub;
            }
            else
            {
                damage += AmmoCharge.med;
            }
        }
        print("NEWWWWW CANNONBALL DAMAGE: " + damage);
    }
}
