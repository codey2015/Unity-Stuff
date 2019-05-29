using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[ExecuteInEditMode]
public class ObjectPlacer : MonoBehaviour {

    [Header("Press left alt + scroll to increase or decrease area of effect.")]
    public Color placerColor = new Color(.75f, .25f, .65f, .4f);
    public bool hideWhenNotSelected = true;
    [Header("Pick an empty gameObject to place everything in.")]
    public Transform placedObjectsHolder;
    [Header("Press shift + the key below to delete objects in the specified layer.")]
    public KeyCode placingKey = KeyCode.M;
    [Header("Puts objects into this layer automatically. Also allows deletion of objects from this layer.")]
    public LayerMask layerUsed;
    public bool useLayerToDelete = true;

    [Range(1, 50)]
    public int objectDensity = 1;

    [Range(.1f, 25)]
    public float scaleOffsetMin = .5f;

    [Range(1, 25)]
    public float scaleOffsetMax = 1.5f;

    private bool hidden = true;
    private void OnDrawGizmos()
    {
        Gizmos.color = placerColor;
        RaycastHit hit;
        if (hideWhenNotSelected)
        {
            if (!Selection.Contains(gameObject))
            {
                hidden = true;
            }
        }
        if (!hideWhenNotSelected)
        {
            hidden = false;
        }
        if (Selection.Contains(gameObject))
        {
            if (Selection.activeObject.Equals(gameObject))
            {
                DeactivateChildren();             
            }
            hidden = false;
        }

        if (hidden == false)
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
            {
                Gizmos.DrawSphere(new Vector3(transform.position.x, hit.point.y, transform.position.z), transform.lossyScale.y);
            }
        }
     }
    
    void DeactivateChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i).position = transform.position;          
        }
    }

    public void MovePlacer(Vector3 pos)
    {
        transform.position = pos;
    }

    public void PlaceObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int x = 0; x < objectDensity; x++)
            {
                GameObject GO = Instantiate(gameObject.transform.GetChild(i).gameObject, transform.position, Quaternion.identity, placedObjectsHolder);
                //GO.transform.parent = placedObjectsHolder;
                GO.SetActive(true);
                GO.transform.position = OffsetVectorPosition(GO.transform.position);
                RaycastHit hitGround;
                if (Physics.Raycast(GO.transform.position, -transform.up, out hitGround, Mathf.Infinity))
                {
                    GO.transform.position = new Vector3(GO.transform.position.x, hitGround.point.y, GO.transform.position.z);
                }
                GO.transform.localScale = OffsetVectorScale(GO.transform.localScale);
                GO.transform.localRotation = OffsetVectorRotation(GO.transform.localRotation);
                GO.name = gameObject.transform.GetChild(i).gameObject.name;

                GO.layer = (int)Mathf.Log(layerUsed.value, 2);

            }
        }
    }

    public void DeleteObjects(Vector3 pos)
    {
        Bounds b = new Bounds
        {
            center = pos,
            extents = new Vector3(transform.lossyScale.y, transform.lossyScale.y, transform.lossyScale.y)
        };

        if (useLayerToDelete)
        {
            foreach (Transform g in placedObjectsHolder)
            {
                if (b.Contains(g.transform.position) && g.gameObject.layer == (int)Mathf.Log(layerUsed.value, 2))
                {
                    DestroyImmediate(g.gameObject);
                }
            }
        }
        if (!useLayerToDelete)
        {
            foreach (Transform g in placedObjectsHolder)
            {
                if (b.Contains(g.transform.position))
                {
                    DestroyImmediate(g.gameObject);
                }
            }
        }
    }

    Vector3 OffsetVectorPosition(Vector3 vec)
    {
        float x = Random.Range(-transform.lossyScale.y, transform.lossyScale.y);
        float z = Random.Range(-transform.lossyScale.y, transform.lossyScale.y);
        Vector3 returnVec = new Vector3(x, vec.y, z);
        returnVec += vec;
        return returnVec;
    }

    Vector3 OffsetVectorScale(Vector3 vec)
    {
        float xyz = Random.Range(scaleOffsetMin, scaleOffsetMax);
        Vector3 returnVec = new Vector3(xyz * vec.x, xyz * vec.y, xyz * vec.z);
        return returnVec;
    }

    Quaternion OffsetVectorRotation(Quaternion vec)
    {
        Quaternion returnVec = new Quaternion(vec.x, Random.rotation.y, vec.z, 1);
        return returnVec;
    }

    public KeyCode PlacingKey()
    {
        return placingKey;
    }

    public void ResizeAOE(float i = 1)
    {
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(i, i, i));
    }
}
