using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class pauseMenu : MonoBehaviour
{


    bool checkUI;
    private int count = 0;
    public Image pausemenu;
    public Image controlsMenu;
    public FirstPersonController me;




    private void Start()
    {
        me = GameObject.FindObjectOfType<FirstPersonController>();
        pausemenu.enabled = false;
        controlsMenu.enabled = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && checkUI == false)
        {
            pausemenu.enabled = true; //colors screen

            checkUI = true;
            count += 1;
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        if (count >= 2 && Time.timeScale == 0 && checkUI == true)
        {
            pausemenu.enabled = false;
            controlsMenu.enabled = false;

            checkUI = false;
            count = 0;
            Time.timeScale = 1;
            Cursor.visible = false;

        }



    }


    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
