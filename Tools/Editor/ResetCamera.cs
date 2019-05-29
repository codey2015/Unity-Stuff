using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetCamera : MonoBehaviour
{
    [MenuItem("Codey's Tools/Reset Camera to (0,0,0)")]
    private static void ResetToZero()
    {
        SceneView.lastActiveSceneView.pivot = Vector3.zero;
    }
}
