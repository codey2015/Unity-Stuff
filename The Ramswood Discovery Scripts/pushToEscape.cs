using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pushToEscape : MonoBehaviour {

    public Transform ob;
    public float moveUp = .5f;
    public float moveDownX = 0f;
    public float moveDownY = -0.05f;
    public float moveDownZ = 0f;
    private float originalY;
    public Vector3 vecCopy;
    public Image press;

    // Use this for initialization
    void Start () {
        if(ob == null)
            ob = GetComponent<Transform>();
        originalY = ob.transform.position.y;
        vecCopy = ob.transform.position;
        press.enabled = false;
    }

    // Update is called once per frame
    void Update () {
	if(ob.transform.position.y <= originalY)
        {
            ob.transform.position = vecCopy;
        }
        ob.transform.Translate(moveDownX, moveDownY, moveDownZ);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ob.transform.Translate(Vector3.up * moveUp);
            }
                press.enabled = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            press.enabled = false;
        }

    }
}
