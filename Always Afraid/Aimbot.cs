using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimbot : MonoBehaviour {
    [Header("Add specified tag to affected objects.")]
    public Color boundsColor = new Color(.9f, .22f, 0f);
    public string autoAimTag = "Auto";
    public float attackRadius = 15f;
    public bool useAsTurret = true;
    [Header("Attach Rigidbody and Collider to projectile. ")]
    public GameObject projectile;
    public float projectileSpeed = 500f;
    public float fireRate = .5f;
    public float destroyAfter = 3f;
    public bool useRaycastDetection = true;
    public float castDistanceOffset = 1f;

    private bool shot = false;
    private bool lockedOn = false;
    private GameObject lockOn;

    private void OnDrawGizmos()
    {
        Gizmos.color = boundsColor;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void Update()
    {
        if (lockedOn)
        {
            transform.LookAt(lockOn.transform);
        }
    }

    void FixedUpdate () {
        if (lockedOn == false)
        {
            foreach (Collider Collider in Physics.OverlapSphere(transform.position, attackRadius))
            {
                if (Collider.gameObject.tag == autoAimTag)
                {
                    transform.LookAt(Collider.transform);
                    lockOn = Collider.gameObject;
                    lockedOn = true;
                    break;
                }
            }
        }

        if(lockedOn == true)
        {
            if (useRaycastDetection == false)
            {
                List<Collider> arr = new List<Collider>();
                foreach (Collider Collider in Physics.OverlapSphere(transform.position, attackRadius))
                {
                    arr.Add(Collider);
                }
                if (!arr.Contains(lockOn.GetComponent<Collider>()))
                {
                    lockedOn = false;
                }
            }
            if(useRaycastDetection == true)
            {
                RaycastHit hit;
                if(Physics.Raycast(projectile.transform.position, projectile.transform.forward * (attackRadius - castDistanceOffset), out hit))
                {
                    if(hit.collider.gameObject.tag != autoAimTag)
                    {
                        lockedOn = false;
                    }
                }
            }
        }

        if (useAsTurret && projectile != null)
        {
            if (shot == false && lockedOn == true)
            {
                StartCoroutine(TurretMode());
            }
        }
        Debug.DrawRay(projectile.transform.position, projectile.transform.forward * (attackRadius - castDistanceOffset), boundsColor);
    }

    IEnumerator TurretMode()
    {
        if (projectile.GetComponent<Rigidbody>() != null)
        {
            GameObject newProjectile = Instantiate(projectile);
            Rigidbody newProjectileRB = newProjectile.GetComponent<Rigidbody>();
            newProjectile.SetActive(true);
            newProjectile.transform.position = projectile.transform.position;
            newProjectileRB.velocity += projectile.transform.forward * projectileSpeed * Time.fixedDeltaTime;
            shot = true;
            yield return new WaitForSeconds(fireRate);            
            shot = false;
            yield return new WaitForSeconds(destroyAfter - fireRate);
            Destroy(newProjectile);
        }
    }
}
