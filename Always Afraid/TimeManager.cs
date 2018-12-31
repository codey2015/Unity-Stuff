
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public string[] slowDownKeys = { "f" };
    public string[] speedUpKeys = { "g" };
    public float timeManipulatorSlowDown = .5f;
    public float timeManipulatorSpeedUp = 1.5f;
    public float changeTimeForSeconds = 2f;
    private bool timeChanged = false;

    // Update is called once per frame
    void Update()
    {
        if(timeChanged == false)
        {
            TimeRunner();
        }
    }

    void TimeRunner()
    {
        foreach (string slowKey in slowDownKeys)
        {
            if (Input.GetKeyDown(slowKey) )
            {
                StartCoroutine(ChangeTimeSlow());
            }
        }

        foreach (string fastKey in speedUpKeys)
        {
            if (Input.GetKeyDown(fastKey))
            {
                StartCoroutine(ChangeTimeFast());
            }
        }
    }

    IEnumerator ChangeTimeFast()
    {
        Time.timeScale *= timeManipulatorSpeedUp;
        timeChanged = true;
        yield return new WaitForSecondsRealtime(changeTimeForSeconds);
        Time.timeScale = 1;
        timeChanged = false;
    }
    IEnumerator ChangeTimeSlow()
    {
        Time.timeScale *= timeManipulatorSlowDown;
        timeChanged = true;
        yield return new WaitForSecondsRealtime(changeTimeForSeconds);
        Time.timeScale = 1;
        timeChanged = false;
    }
}
