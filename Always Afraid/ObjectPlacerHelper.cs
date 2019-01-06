using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPlacer))]
public class ObjectPlacerHelper : Editor
{
    void OnSceneGUI()
    {
        ObjectPlacer OP = (ObjectPlacer)target;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            OP.MovePlacer(hit.point);
        }

        Event e = Event.current;

            if(e.alt)
            {
                if (e.isScrollWheel)
                {
                    if (e.delta[1] < 0f)
                        {
                            OP.ResizeAOE(.7f);
                        }
                        else if (e.delta[1] > 0f)
                        {
                            OP.ResizeAOE(1.3f);
                        }
                }
            }
       
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (OP.PlacingKey()))
                    {
                        OP.PlaceObject();                       
                    }
                    break;
                }
         
        }
        

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ObjectPlacer OP = (ObjectPlacer)target;
        

        EditorGUILayout.PropertyField(serializedObject.FindProperty("exampleClassList"), new GUIContent("Examples Lists", "List of clasess."), true);
        ObjectShowerHelper.ShowWindow();
        serializedObject.ApplyModifiedProperties();
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }
    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        base.OnPreviewGUI(r, background);
        GUI.Label(r, target.name + " is being previewed");
        //GUI.
    }

}
