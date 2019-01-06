using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Create an editor window which can display a chosen GameObject.
// Use OnInteractivePreviewGUI to display the GameObject and
// allow it to be interactive.

public class ObjectShowerHelper : EditorWindow
{
    static GameObject gameObject;
    Editor gameObjectEditor;
    //ObjectPlacerHelper gameObjectEditor2;
    private float t = 0;
    private GameObject GOTemp;

    [MenuItem("ObjectPlacer/GameObject Editor")] 
    public static void ShowWindow()
    {
        GetWindowWithRect<ObjectShowerHelper>(new Rect(0, 0, 256, 256));
        //GUI.DragWindow(new Rect(0, 0, 100000, 1000000));
    }

    public void OnGUI()
    {
        t += Time.deltaTime;


        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);
        if (t > 1f)
        {
            GOTemp = gameObject;
            t = 0;
        }

        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;

        
        if (gameObjectEditor == null || GOTemp != gameObject)
            gameObjectEditor = Editor.CreateEditor(gameObject);
        

        

        if (gameObject != null)
        {
            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
        }
        //GUI.DragWindow(new Rect(0, 0, 100000, 1000000));
        //ShowWindow();

    }

    GameObject GetGO(GameObject GO)
    {
        return GO;
    }
}