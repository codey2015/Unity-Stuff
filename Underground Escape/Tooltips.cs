using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltips : MonoBehaviour
{
    public string tooltip = "My tooltip";
    public int fontsizzze = 15;

    public int allowableWidth = 150;
    public int allowableHeight = 150;

    public int xPos = 50;
    public int yPos = 50;


    void Start () {

    }

    void Update() {
        
    }

        public void OnGUI()
    {

            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            myStyle.fontSize = fontsizzze;
            myStyle.normal.textColor = Color.yellow;
            GUI.Box(new Rect(xPos, yPos, allowableWidth, allowableHeight), tooltip, myStyle); 
    }
    
}
