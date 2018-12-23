using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLevitation : MonoBehaviour {

    [Header("Put affected layer masks in array. Objects without layers may be counted.")]
    public List<int> layerMasks = new List<int> { 9, 10 };
    public float maxDistanceToGrab = 10f;
    public Transform castFrom;
    public Color rayCastColor = new Color (0, .9f, .3f);
    [Header("Turn off root motion for animated objects to make them fall with gravity.")]
    public string[] keys = { "e" };
    public string[] forcePushKeys = { "q" };
    public float xForce = 100f;
    public float yForce = 100f;
    public float zForce = 100f;

    void Start ()
    {
		if(castFrom == null)
        {
            castFrom = gameObject.transform;
        }
	}
	
	void FixedUpdate () {
        RaycastHit hit;
        //use ~ operator to only cast to the specified mask
        if (Physics.Raycast(castFrom.position, castFrom.forward, out hit, maxDistanceToGrab, ~cast()))
        {
            if (hit.transform.GetComponent<Rigidbody>())
            {
                lev(hit);
            }
        }
        Debug.DrawRay(castFrom.position, castFrom.forward * maxDistanceToGrab, rayCastColor);       
    }

    int cast()
    {
        for (int i = 0; i < layerMasks.Count; i++)
        {
            return 1 << i;
        }
        return 1 << layerMasks[0]; 
    }

    void lev(RaycastHit hit)
    {
        foreach (string key in keys)
        {
            if (Input.GetKey(key))
            {
                //hit.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
                if ((castFrom.localEulerAngles.y > 180 && castFrom.localEulerAngles.y < 270) || castFrom.localEulerAngles.y < 90)
                {
                    hit.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(hit.transform.position.x, hit.point.y, hit.point.z));
                }
                if ((castFrom.localEulerAngles.y > 90 && castFrom.localEulerAngles.y < 180) || castFrom.localEulerAngles.y > 270) //&& castFrom.transform.localEulerAngles.y < 270)
                {
                    hit.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(hit.point.x, hit.point.y, hit.transform.position.z));
                }
            }
            foreach (string fpKey in forcePushKeys)
            {
                if (Input.GetKeyUp(fpKey))
                {
                    Vector3 forwardForce = (new Vector3(xForce * castFrom.forward.x, yForce * castFrom.forward.y, zForce * castFrom.forward.z));

                    hit.transform.GetComponent<Rigidbody>().AddForce(forwardForce);
                    hit.transform.GetComponent<Rigidbody>().velocity += forwardForce;
                }
            }
        }
    }
}
