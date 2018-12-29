using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLevitation : MonoBehaviour {

    [Header("Put affected layer masks in array. Objects without layers may be counted.")]
    public List<int> layerMasks = new List<int> { 9, 10 };
    public float maxDistanceToGrab = 10f;
    public Transform castFrom;
    public Color rayCastColor = new Color (0, .9f, .3f);
    public bool requireRigidbody = true;
    [Header("Turn off root motion for animated objects to make them fall with gravity.")]
    public string[] keys = { "e" };
    public string[] forcePushKeys = { "q" };
    public float xForce = 50f;
    public float yForce = 50f;
    public float zForce = 50f;
    [Header("Use Force instead of velocity to account for mass.")]
    public bool useForce = true;
    private float zPoint;
    private float xPoint;
    private float distance;
    private Rigidbody obj;
    private bool pickedUp = false;

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
            lev(hit);
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
            if (Input.GetKeyDown(key)){
                distance = hit.distance + 1.44f;
                pickedUp = true;
                try
                {
                    obj = hit.transform.GetComponent<Rigidbody>();
                }
                catch
                {
                    obj = null;
                }
                
            }
            if (Input.GetKey(key) && pickedUp == true)
            {
                if(obj != null && requireRigidbody == true)
                {
                    obj.MovePosition(castFrom.position + castFrom.forward * distance);
                }
                if(requireRigidbody == false)
                {
                    hit.transform.position = castFrom.position + castFrom.forward * distance;
                }
                
            }
            foreach (string fpKey in forcePushKeys)
                {
                    if (Input.GetKeyUp(fpKey))
                    {                        
                        if (requireRigidbody == true)
                        {
                            pickedUp = false;
                            Vector3 forwardForce = (new Vector3(xForce * castFrom.forward.x, yForce * castFrom.forward.y, zForce * castFrom.forward.z));
                            if (useForce)
                            {
                                hit.transform.GetComponent<Rigidbody>().AddForce(forwardForce * 100);
                            }
                            else
                            {
                                hit.transform.GetComponent<Rigidbody>().velocity += forwardForce;
                            }
                        }
                }
            }
        }
    }
}
