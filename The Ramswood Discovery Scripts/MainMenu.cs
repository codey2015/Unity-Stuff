using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour {

    public Image background;
    public AudioSource music;
    public Image journal;
    public Button play;
    public Text playTxt;
    public Image im;
    public bool GO;

    public void Start()
    {
        if (GO == false)
        {
            play.enabled = false;
            playTxt.enabled = false;
            im.enabled = false;
        }
    }

    public void playGame()
    {
        music.enabled = false;
        background.enabled = true;
        journal.enabled = true;
        play.enabled = true;
        playTxt.enabled = true;
        im.enabled = true;
    }

    public void startGame()
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

	public void MM()
	{
		SceneManager.LoadScene(0);
	}
	public void SG()
	{
		SceneManager.LoadScene(1);
	}
}
