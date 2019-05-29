using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllWithNoMeshRender : MonoBehaviour
{
    [MenuItem("Codey's Tools/Select All Objects With Off mesh renderer")]
    private static void GetObjects()
    {
        GameObject[] m_Objects = FindObjectsOfType<GameObject>();
        List<GameObject> selectable = new List<GameObject>();
        foreach(GameObject o in m_Objects)
        {
            try
            {
                Renderer rend = o.GetComponent<Renderer>();
                if (rend.enabled == false)
                {
                    selectable.Add(o);
                }
            }
            catch { }
        }
        GameObject[] tempArray = selectable.ToArray();
        Selection.objects = tempArray;
    }

}
