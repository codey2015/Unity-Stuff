using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.CharacterController;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class lives : MonoBehaviour
{
    public int livesCount = 5;
    public int ID;
    bool check = true;
    public string preface = "Lives: ";
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = preface + livesCount;
    }

    // Update is called once per frame
    void Update()
    {

       if (Invector.CharacterController.vThirdPersonController.instance.currentHealth == Invector.CharacterController.vThirdPersonController.instance.maxHealth)
        {
           check = true;
       }
   
        if (check == true)
        { 
        NumOfLives();
            print("CHECK==TRUE");
        }


    }

    public void NumOfLives()
    {
        if (Invector.CharacterController.vThirdPersonController.instance.currentHealth <= 0)
        {
            livesCount -= 1;
            check = false;

            if (livesCount <= 0)
            {
                Invector.vGameController.instance.spawnPoint.position = this.transform.position;
                Invector.vGameController.instance.spawnPoint.rotation = this.transform.rotation;
                SceneManager.LoadScene(ID);
            }

        }
    }

    private void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("box"));
        myStyle.fontSize = 17;
        myStyle.normal.textColor = Color.white;
        GUI.Box(new Rect(Screen.width / 2.09f, 40, 83, 28), preface + " " + livesCount, myStyle);

    }

}