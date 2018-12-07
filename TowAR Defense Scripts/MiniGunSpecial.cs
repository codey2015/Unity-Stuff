using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGunSpecial : towerAttack {

    [HideInInspector]
    public Transform target2;
    [HideInInspector]
    public Enemy targetEnemy2;

    public Transform firePoint2;

    public float damage2;
    public float TempDmg2;
    GameObject cannonBallGO2;

    protected override void Start()
    {
        base.Start();
        damage2 = damage;
        TempDmg2 = damage2;
        //firePoint2 = firePoint;
    }

    protected override void Update()
    {
        base.Update();

        if (target2 == null)
            target2 = target;


        if (fireCountDown <= 0)
        {
            if (CanSeeEnemy2(target2.gameObject))
            {
                Shoot2();
            }
        }


        if (cannonBallGO2 != null)
        {
            Seek2(target2);
            cannonBallGO2.transform.LookAt(target);
            Vector3 vec = (firePoint2.transform.position - targetEnemy2.transform.position) * maxDistanceMultiplier;
            
            cannonBallGO2.transform.Translate(-vec * Time.deltaTime * maxDistanceMultiplier * fineTuneSpeed, Space.World);

            float distanceToEnemy2 = Vector3.Distance(cannonBallGO2.transform.position, targetEnemy.transform.position);
            if (distanceToEnemy2 < hurtAt)
            {
                Destroy(cannonBallGO2);
                //damage enemy

                Damage2(targetEnemy2.transform);
            }
            else
            {
                StartCoroutine(destroyCannonBall(cannonBallGO2));
            }

        }
    }

    
    // new version of method to keep track of second target
    protected new void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        GameObject secondNearestEnemy = null;

        float shortestDistance = Mathf.Infinity;
        float secondShortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                secondShortestDistance = shortestDistance;
                shortestDistance = distanceToEnemy;
                secondNearestEnemy = nearestEnemy;
                nearestEnemy = enemy;
                if (nearestEnemy)
                {
                    newTarget = true;
                    print("NEW TARGET");
                }
            }
        }

        // have second projectile still be used if a second enemy isn't present in the tower's range
        if (secondNearestEnemy == null || secondShortestDistance > range)
            secondNearestEnemy = nearestEnemy;

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            target2 = secondNearestEnemy.transform;
            newTarget = true;
            print("NEW TARGET");
        }
        else
        {
            target = null;
        }
        if (target != null)
        {
            targetEnemy = target.GetComponent<Enemy>();
            targetEnemy2 = target2.GetComponent<Enemy>();
        }
    }


   
    public void Seek2(Transform _target)
    {
        target2 = _target;
    }
    //############ Shoot funtion used for launching projectiles ############//

    void Shoot2()
    {
        cannonBallGO2 = (GameObject)Instantiate(cannonBallPrefab, firePoint2.position, firePoint2.rotation);

        if (cannonBallGO2 != null)
        {

            if (strongerAmmo == true && AmmoCharge.compareOnce == true)
            {
                compareColors2();
                if (newTarget == true)
                {
                    damage2 = TempDmg2;
                    compareColors2();
                    newTarget = false;
                    AmmoCharge.compareOnce = true;
                }
            }
        }
    }

    bool CanSeeEnemy2(GameObject target)
    {
        RaycastHit info;
        Debug.DrawLine(firePoint2.transform.position, targetEnemy2.transform.position);
        if (Physics.Raycast(firePoint2.transform.position, targetEnemy2.transform.position - firePoint2.transform.position, out info, range))
        {
            Debug.DrawLine(gameObject.transform.position, target2.transform.position, Color.cyan);
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

    void Damage2(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage2);
        }
    }


    //############ Used to compare the colors of the enemy and the tower ############//

    void compareColors2()
    {
        print("TowerColor: " + TowerColor);
        print("EnemyColor: " + targetEnemy.EnemyColor);
        if (TowerColor == colorOptions.Red)
        {
            print("TowerColor == colorOptions.Red");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Green)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Green");
                damage2 += AmmoCharge.add;
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Blue)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Blue");
                damage2 += AmmoCharge.sub;
            }
            else
            {
                damage2 += AmmoCharge.med;
            }
        }

        if (TowerColor == colorOptions.Blue)
        {
            print("TowerColor == colorOptions.Blue");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Red)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Red");
                damage2 += AmmoCharge.add;
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Green)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Green");
                damage2 += AmmoCharge.sub;
            }
            else
            {
                damage2 += AmmoCharge.med;
            }
        }

        if (TowerColor == colorOptions.Green)
        {
            print("TowerColor == colorOptions.Green");
            if (targetEnemy.EnemyColor == Enemy.colorOptions.Blue)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Blue");
                print("DAMAGE: " + damage2);
                damage2 += AmmoCharge.add;
                print("DAMAGE2: " + AmmoCharge.add);
                print("New Damage: " + damage2);
            }

            if (targetEnemy.EnemyColor == Enemy.colorOptions.Red)
            {
                print("targetEnemy.EnemyColor == Enemy.colorOptions.Red");
                damage2 += AmmoCharge.sub;
            }
            else
            {
                damage += AmmoCharge.med;
            }
        }
        print("NEWWWWW CANNONBALL DAMAGE: " + damage2);
        AmmoCharge.compareOnce = false;
    }
  
}