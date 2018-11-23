using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class pause : MonoBehaviour
{

    public Image pauseMenu;
    public Image controlsMenu;

    public Button unpausebutton;
    public Button exitbutton;
    public Button controlsbutton;

    public Text unPausebtnTxt;
    public Text ExitbtnTxt;
    public Text controlsbtnTxt;
    public Text controlsMenuTxt;

    public Button backButtonControls;
    public Text backTxtControls;

    private int count = 0;

    public Camera cam;
    public bool checkUI;
    public FirstPersonController me;
    //public Button unPause;

    // Use this for initialization
    void Start()
    {
        pauseMenu.enabled = false; //colors screen
        controlsMenu.enabled = false;

        disableAll();

        Cursor.visible = false;
        me = GameObject.FindObjectOfType<FirstPersonController>();
        checkUI = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1 && checkUI == false && count == 0)
        {
            pauseMenu.enabled = true;

            unpausebutton.enabled = true;
            exitbutton.enabled = true;
            controlsbutton.enabled = true;

            unPausebtnTxt.enabled = true;
            ExitbtnTxt.enabled = true;
            controlsbtnTxt.enabled = true;


            checkUI = true;
            count += 1;
            Time.timeScale = 0;
            me.enabled = false;
            Cursor.visible = true;
            //unPause.onClick.Equals(Time.timeScale = 0);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && count >= 2 && Time.timeScale == 0 && checkUI == true)
        {
            pauseMenu.enabled = false;
            controlsMenu.enabled = false;

            disableAll();

            checkUI = false;
            count = 0;
            Time.timeScale = 1;
            me.enabled = true;
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

        }
        


    }

    public void UnPause()
    {
        Time.timeScale = 1;
        count = 0;
        me.enabled = true;

        pauseMenu.enabled = false;

        disableAll();

        checkUI = false;
        Cursor.visible = false;
    }






    private void disableAll()
    {


        unpausebutton.enabled = false; //unpause
        unPausebtnTxt.enabled = false;

        exitbutton.enabled = false; //exit
        ExitbtnTxt.enabled = false;

        controlsbutton.enabled = false; //controls
        controlsbtnTxt.enabled = false;
        controlsMenuTxt.enabled = false;

        backButtonControls.enabled = false; //back to pause from controls
        backTxtControls.enabled = false;
    }

    private void enableAll()
    {


        unpausebutton.enabled = true;//unpause
        unPausebtnTxt.enabled = true;

        exitbutton.enabled = true;//exit
        ExitbtnTxt.enabled = true;

        controlsbutton.enabled = true; //controls
        controlsbtnTxt.enabled = true;

    }

    public void controlsMenuEnabled() //enable menu, put back button to go back to previous menu
    {
        pauseMenu.enabled = false;
        disableAll();
        controlsMenu.enabled = true;
        backButtonControls.enabled = true;
        backTxtControls.enabled = true;
        controlsMenuTxt.enabled = true;
    }

    public void controlsMenuDisabled() //enable menu, put back button to go back to previous menu
    {
        pauseMenu.enabled = true;
        controlsMenu.enabled = false;

        enableAll();

        backButtonControls.enabled = false;
        backTxtControls.enabled = false;
        controlsMenuTxt.enabled = false;
    }


    public void exitGame()
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
