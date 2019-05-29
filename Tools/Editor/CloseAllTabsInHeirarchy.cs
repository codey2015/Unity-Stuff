using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CloseAllTabsInHeirarchy : MonoBehaviour
{
    [MenuItem("Codey's Tools/Collapse Heirarchy And Save Scene")]
    private static void CloseTabs()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(EditorSceneManager.GetSceneByName(EditorSceneManager.GetActiveScene().name).path);
    }
}
