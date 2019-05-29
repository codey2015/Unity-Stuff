using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObject))]
public class PlayMusicInEditor : Editor
{


    [MenuItem("Codey's Tools/INFO ONLY: Press + to activate sound on current object and - to turn off all sounds when an object is selected")]
    private static void Info() { }

    void OnSceneGUI()
    {
        Event e = Event.current;
        try
        {
            if (e.isKey)
            {
                if (e.character == '-' || e.character == '_')
                {
                    AudioSource[] a = FindObjectsOfType<AudioSource>();
                    foreach (AudioSource ad in a)
                    {
                        ad.Stop();
                    }
                }
                if (e.character == '=' || e.character == '+')
                {
                    if (!Selection.activeTransform.GetComponent<AudioSource>().isPlaying)
                    {
                        Selection.activeTransform.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
        catch { }
    }
}
