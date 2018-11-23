using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraScript : MonoBehaviour {

    private int count = 0;
    public float range = 20;

    private int jumpScare = 0;

    public Image image;
    public Camera cam;
    public AudioSource sound;
    public Light myLight;
    public AudioSource laugh;

    public int objectiveCount = 0;
    public bool checkUI = false;
    public bool checkUI2 = false;

    private Shader shade;
    private Renderer rend;

    void Start () {
        image.enabled = false;
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.C) && Time.timeScale == 1 && checkUI2 == false)
        {
            image.enabled = true;
            checkUI = true;
            count += 1;
        }
        if(count >= 2)
        {
            image.enabled = false;
            checkUI = false;
            count = 0;
        }

        if (Input.GetButtonDown("Fire1") && checkUI == true && Time.timeScale == 1)
        {
            checkCam();
            sound.Play();
            myLight.enabled = true;
            jumpScare += 1;
            if(jumpScare == 10)
            {
                //then pop up sudden scary image/audio
                laugh.Play();
            }
        }
        else
        {
            StartCoroutine(Flash());
        }

        if(objectiveCount == 10)
        {
            checkUI = false;
            checkUI2 = true;
        }
    }

    IEnumerator Flash()
    {

        yield return new WaitForEndOfFrame();
        myLight.enabled = false;
    }
	
    void checkCam()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if(hit.transform.tag == "ObjectiveItem")
            {
                shade = GetComponent<Shader>();
                shade = Shader.Find("Standard");
                rend = hit.transform.GetComponent<Renderer>();
                rend.material.shader = shade;
                if (hit.transform.parent != null)
                {
                    foreach (Transform child in hit.transform.parent)
                    {
                        Debug.Log(child);

                        if (child.GetComponent<Renderer>() != null)
                        {
                            rend = child.GetComponent<Renderer>();
                            rend.material.shader = shade;
                        }
                    }
                }
                objectiveCount += 1;
                hit.transform.tag = "obtained";
            }
            Debug.Log(hit.transform.name);
        }
    }


    void OnGUI()
    {
        if (checkUI == true && Time.timeScale == 1 && checkUI2 == false)
        {
            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("box"));
            string preface = "Useful photographs: ";
            myStyle.fontSize = Screen.height / 35;
            myStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(Screen.width / 10.0f, Screen.height / 1.3f, Screen.width / 3.2f, Screen.height / 20), preface + " " + objectiveCount, myStyle); // Inventory
        }
    }
}
