using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour {
    public string[] keys = { "z" };
    public float attractFor = 1.5f;
    public float xForce = 1000f;
    public float yForce = 1000f;
    public float zForce = 1000f;
    public float pullRadius = 2;
    public float pullForce = 1;

    public GameObject gravityObject;
    public GameObject shootFrom;

    private GameObject GO;
    private bool donePulling = true;
	// Update is called once per frame
	void FixedUpdate () {
        if (donePulling)
        {
            shoot();
            
        }
        if(GO != null)
        {
            FindColliders();
        }
	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(GO != null)
        {
            Gizmos.DrawWireSphere(GO.transform.position, pullRadius);
        }
    }

    void shoot()
    {
        foreach (string key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(Attract());
            }
        }
       
    }

    void FindColliders()
    {
        if (GO != null)
        {
            foreach (Collider myCollider in Physics.OverlapSphere(GO.transform.position, pullRadius))
            {
                if (myCollider.tag != "Player" && myCollider.tag != "Terrain" && myCollider.GetComponent<Rigidbody>() != null && myCollider.gameObject!= GO)
                {
                    Vector3 forceDirection = GO.transform.position - myCollider.transform.position;
                    myCollider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
                    GO.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GO.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                }
            }
        }
    }

    IEnumerator Attract()
    {
        donePulling = false;
        GO = Instantiate(gravityObject);
        GO.transform.position = gravityObject.transform.position;
        GO.SetActive(true);
        Rigidbody RB = GO.GetComponent<Rigidbody>();
        Vector3 forwardForce = (new Vector3(xForce * shootFrom.transform.forward.x, yForce * shootFrom.transform.forward.y, zForce * shootFrom.transform.forward.z));
        RB.AddForce(forwardForce);
        yield return new WaitForSeconds(attractFor);
        Destroy(GO);
        donePulling = true;
    }
   
}
