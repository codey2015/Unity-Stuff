using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class objectivesFinished : MonoBehaviour {

    private CameraScript camerascript;
    public bool getOut = false;
    public int OC = 10;
    private bool temp = false;

    public AudioSource rushMusic;

    public AudioListener audioListener;
    public List<AudioSource> audioSources;

    public GameObject boss;

    // Use this for initialization
    void Start () {
        camerascript = GetComponent<CameraScript>();
        boss.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
        if(camerascript.objectiveCount == OC && Time.timeScale == 1 && temp == false)
        {

            Debug.Log("LoadScene");
            getOut = true;
            camerascript.checkUI = false;
            camerascript.image.enabled = false;

            if (temp == false)
            {
                foreach (AudioSource audioSorce in audioSources)
                {
                    audioSorce.Stop();
                }
            }
            temp = true;
            StartCoroutine(PlayRushMusic());
            //SceneManager.LoadScene("mainMenu");

            //Cursor.visible = true;
            if (boss != null)
            {
                boss.SetActive(true);
            }
        }

        if(temp == true)
        {
            camerascript.checkUI = false;
            camerascript.image.enabled = false;
            camerascript.myLight.enabled = false;
            camerascript.sound.enabled = false;
        }

    }

    IEnumerator PlayRushMusic()
    {
        yield return new WaitForSeconds(10);
        rushMusic.Play();
        
    }

    void OnGUI()
    {
        if ( Time.timeScale == 1 && getOut == true)
        {
            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("box"));
            string preface = "GET TO THE TRUCK!!!";
            myStyle.fontSize = Screen.height / 35;
            myStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(Screen.width / 10.0f, Screen.height / 1.3f, Screen.width / 3.2f, Screen.height / 20), preface  , myStyle); // Inventory
        }

    }
}
