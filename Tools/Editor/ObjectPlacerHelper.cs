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
        if (e.shift)
        {
            if (Event.current.keyCode == (OP.PlacingKey()))
            {
                OP.DeleteObjects(hit.point);
            }
        }
       
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (OP.PlacingKey()) && !e.shift)
                    {
                        OP.PlaceObject();                       
                    }
                    break;
                }
         
        }
        

    }
