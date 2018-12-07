using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleSpecial : towerAttack
{
    public float dotDelay = 1f;
    public float dotDamage = 10f;
    public float dotTicks = 3f;

    public ParticleSystem burnVFX;

    protected override void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.damagedBy = damage;
            e.TakeDamage(damage);

            ParticleSystem burnEffect = Instantiate(burnVFX, enemy.position, enemy.rotation);
            burnEffect.transform.SetParent(enemy, true);
            Destroy(burnEffect);

            StartCoroutine(DOT(e, burnEffect));
        }
    }

    IEnumerator DOT(Enemy e, ParticleSystem burn, int i = 0)
    {
        if (i < dotTicks)
        {
            yield return new WaitForSeconds(dotDelay);
            if (e)
            {
                e.TakeDamage(dotDamage);
                StartCoroutine(DOT(e, burn, ++i));
            }
        } else
        {
            Destroy(burn);
        }
    }
}