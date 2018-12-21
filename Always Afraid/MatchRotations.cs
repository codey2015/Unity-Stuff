using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotations : MonoBehaviour
{

    public GameObject matchRotation;
    public bool matchX = true;
    public bool matchY = true;
    public bool matchZ = true;
    [Header("Extra Options")]
    public bool reverseSelected = false;
    public bool rotateToPosition = false;

    void Update()
    {
        if (matchRotation != null)
        {
            checkBools();
        }
    }

    void checkBools()
    {
        float x = checkBool(matchX)[0];
        float y = checkBool(matchY)[1];
        float z = checkBool(matchZ)[2];
        if (rotateToPosition == false)
        {
            x = checkBool(matchX)[0];
            y = checkBool(matchY)[1];
            z = checkBool(matchZ)[2];
        }
        if (rotateToPosition == true)
        {
            x = checkPos(matchX)[0];
            y = checkPos(matchY)[1];
            z = checkPos(matchZ)[2];
        }
        if (reverseSelected == false)
        {
            if (rotateToPosition == false)
            {
                gameObject.transform.rotation = new Quaternion(x, y, z, matchRotation.transform.rotation.w);
            }
            if (rotateToPosition == true)
            {
                gameObject.transform.LookAt(new Vector3(x, y, z));
            }
        }
        if (reverseSelected == true)
        {
            if (rotateToPosition == false)
            {
                gameObject.transform.rotation = new Quaternion(-x, -y, -z, matchRotation.transform.rotation.w);
            }
            if (rotateToPosition == true)
            {
                //gameObject.transform.rotation = new Quaternion(-x, -y, -z, matchRotation.transform.rotation.w);

                Vector3 vec = new Vector3(x, y, z);
                //print(vec);
                gameObject.transform.LookAt(-vec, vec);
            }
        }
    }

    Quaternion checkBool(bool rot)
    {
        if (rot == true)
        {
            return matchRotation.transform.rotation;
        }
        else
        {
            return gameObject.transform.rotation;
        }
    }
    Vector3 checkPos(bool rot)
    {
        if (rot == true)
        {
            return matchRotation.transform.position;
        }
        else
        {
            return gameObject.transform.position;
        }
    }
}
