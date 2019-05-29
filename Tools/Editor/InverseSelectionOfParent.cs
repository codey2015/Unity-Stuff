using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class InverseSelectionOfParent : MonoBehaviour
{
    [MenuItem("Codey's Tools/Inverse Selection (In reference to parent)")]
    private static void InverseSelection()
    {
        try
        {
            GameObject[] activeObjects = Selection.gameObjects;
            GameObject aoParent = activeObjects[0].transform.parent.gameObject;

            List<GameObject> selection = new List<GameObject>();
            for (int i = 0; i < aoParent.transform.childCount; i++)
            {
                selection.Add(aoParent.transform.GetChild(i).gameObject);
            }
            GameObject[] inversedArray = selection.Except(activeObjects).ToArray();
            Selection.objects = inversedArray;
        }
        catch(System.Exception e)
        {
            print(e);
        }
    }
}
