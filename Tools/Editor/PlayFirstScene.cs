using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class PlayFirstScene : MonoBehaviour
{
    [MenuItem("Codey's Tools/Play First Scene")]
    private static void StartFirstScene()
    {
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        EditorApplication.isPlaying = true;
    }
}
