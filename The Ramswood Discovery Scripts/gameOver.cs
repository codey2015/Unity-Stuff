using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour {


    public int CaughtCounter = 0; //used for game over
    bool doOnce = false;
    public int timesCaught = 3;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (CaughtCounter >= timesCaught)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // if CaughtCounter int reaches 3 scene changes to game over scene
            Cursor.visible = true;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (doOnce == false)
            {
                CaughtCounter++; // adds to CaughtCounter int
                doOnce = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doOnce = false;
        }

    }




}




