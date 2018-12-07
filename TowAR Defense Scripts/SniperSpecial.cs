using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperSpecial : towerAttack {

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
        firePoint2 = firePoint;
    }

    protected override void Update()
    {
        base.Update();

        // shoot same target unless enemy dead
        if (target != null && targetEnemy != null)
        {
            target2 = target;
            targetEnemy2 = targetEnemy;
        } else
        {
            //Destroy(cannonBallGO2);
            cannonBallGO2 = null;
            /* *** NOTE FOR AFTER ALPHA: find a way to let the projectiles continue to travel onward
             *     instead of instantly destroying or freezing (both first and second)
             */
        }


        if (fireCountDown <= 0)
        {
            //if (CanSeeEnemy2(target2.gameObject))
            {
                StartCoroutine(Shoot2());
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
    protected override void UpdateTarget()
    {
        base.UpdateTarget();
        target2 = target;
    }


   
    public void Seek2(Transform _target)
    {
        target2 = _target;

    }
    //############ Shoot funtion used for launching projectiles ############//

    IEnumerator Shoot2()
    {
        // delay second shot
        yield return new WaitForSeconds(0.2f);

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

    /*bool CanSeeEnemy2(GameObject target)
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
    }*/

    protected new void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            if((e.health - damage) <= 0)
            {
                target2 = null;
                targetEnemy2 = null;
                Destroy(cannonBallGO2);
                cannonBallGO2 = null;
            }
            e.TakeDamage(damage);
        }

    }

    void Damage2(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage2 / 2);  // halved for second shot
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