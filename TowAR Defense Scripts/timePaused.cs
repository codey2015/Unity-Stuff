using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timePaused : MonoBehaviour {

    public bool unpaused = false;

    private float currentPaused = 0;

    public void pauseGame()
    {
        checkPauseStatus();
        Time.timeScale = currentPaused;
    }

    public void unPauseGame()
    {
        checkPauseStatus();
        Time.timeScale = currentPaused;
    }

    private void checkPauseStatus()
    {
        if (unpaused == false)
        {
            currentPaused = 0;
        }

        if (unpaused == true)
        {
            currentPaused = 1;
        }
    }
}
