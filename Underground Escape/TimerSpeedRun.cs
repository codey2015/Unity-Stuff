using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.SceneManagement;

public class TimerSpeedRun : MonoBehaviour {
    public string tooltip = "My tooltip";
    public int fontsizzze = 15;

    public int allowableWidth = 150;
    public int allowableHeight = 150;

    public float xPos = 50;
    public float yPos = 50;

    public int ID = 0;

    private bool check = false;
    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
    }
      void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown("p"))
            {
                check = true;
            }
        }
    }

    void OnGUI()
    {
        if (check == true)
        {

            int minutes = Mathf.FloorToInt(Time.time / 60F);
            int seconds = Mathf.FloorToInt(Time.time - minutes * 60);
            string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            string excellentTime = "This would be an excellent time! That is if you finish anytime soon.";
            string veryGoodTime = "If you're near the end, you're doing very well! If not...";
            string goodTime = "Hey... that's pretty good!";

            string decentTime = "You're making half decent time if you're near the end.";


            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            myStyle.fontSize = fontsizzze;
            myStyle.normal.textColor = Color.yellow;
            GUI.Box(new Rect((Screen.width)/xPos, (Screen.height) / yPos, allowableWidth, allowableHeight), tooltip + formattedTime, myStyle);


            if (minutes == 2 && seconds >= 0 && seconds <= 5)
            {
                myStyle.fontSize = fontsizzze;
                myStyle.normal.textColor = Color.yellow;
                //GUI.Box(new Rect((Screen.width) / xPos, (Screen.height) / yPos + 50, allowableWidth, allowableHeight), excellentTime, myStyle);
            }
            if (minutes == 5 && seconds >= 0 && seconds <= 5)
            {
                myStyle.fontSize = fontsizzze;
                myStyle.normal.textColor = Color.yellow;
                GUI.Box(new Rect((Screen.width) / xPos, (Screen.height) / yPos + 50, allowableWidth, allowableHeight), veryGoodTime, myStyle);
            }

            if (minutes == 7 && seconds >= 0 && seconds <= 5)
            {
                myStyle.fontSize = fontsizzze;
                myStyle.normal.textColor = Color.yellow;
                GUI.Box(new Rect((Screen.width) / xPos, (Screen.height) / yPos + 50, allowableWidth, allowableHeight), goodTime, myStyle);
            }

            if (minutes == 10 && seconds >= 0 && seconds <= 5)
            {
                myStyle.fontSize = fontsizzze;
                myStyle.normal.textColor = Color.yellow;
                GUI.Box(new Rect((Screen.width) / xPos, (Screen.height) / yPos + 50, allowableWidth, allowableHeight), decentTime, myStyle);
            }

            
        }
    }


}
