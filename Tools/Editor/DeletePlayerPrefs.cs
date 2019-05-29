using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefs : EditorWindow
{
    [MenuItem("Codey's Tools/Clear All PlayerPrefs")]
    private static void DeleteAllPlayerPrefs()
    {
        DeletePlayerPrefs window = ScriptableObject.CreateInstance<DeletePlayerPrefs>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 300, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Are you sure you want to delete all Player data?", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("Yes"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted all player prefs.");
            this.Close();
        }

        if (GUILayout.Button("No"))
        {
            this.Close();
        }
    }
}
