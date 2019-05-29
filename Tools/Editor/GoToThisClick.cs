using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class GoToThisClick : Editor {

    private bool m_Zooming = false;
    private float t = 0;
    public static bool s_Toggle = true;

    [MenuItem("Codey's Tools/INFO ONLY: Press alt + left click to zoom once any object is selected. Toggle On/Off with capslock")]
    private static void Info(){}

    void OnSceneGUI()
    {
        Transform OP = (Transform)target;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Event.current.capsLock && Event.current.type == EventType.KeyDown)
        {
            Event.current.type = EventType.KeyUp;
            s_Toggle = !s_Toggle;
            //Debug.Log(s_Toggle);
        }


        if (s_Toggle)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Event e = Event.current;

                if (e.alt)
                {
                    if (e.type == EventType.MouseUp)
                    {
                        m_Zooming = true;
                    }
                }

                if (Vector3.Distance(ray.origin, hit.point) < 10f)
                {
                    m_Zooming = false;
                    t = 0;
                }
            }

            if (m_Zooming)
            {
                t += Time.deltaTime;
                SceneView.currentDrawingSceneView.LookAt(hit.point);
                SceneView.currentDrawingSceneView.pivot += Time.deltaTime * 10 * ray.direction;
                SceneView.lastActiveSceneView.Frame(new Bounds(hit.point, Vector3.one), false);
                SceneView.currentDrawingSceneView.Repaint();

                if (t > 1000f)
                {
                    m_Zooming = false;
                    t = 0;
                }
            }
        }
    }
}
