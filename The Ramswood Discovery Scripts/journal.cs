using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class journal : MonoBehaviour
{
    public Image journalot;//this is for the beginning when text tells the player to open the journal

    public Image journal1; //journal image

    //clues
    public Image Clue00;
    public Image Clue01;
    public Image Clue02;
    public Image Clue03_04;
    public Image Clue05;
    public Image Clue06;
    public Image Clue07;
    public Image Clue08;
    public Image Clue09;
    public Image Clue10;

    private bool journEnabled = false;//check to see if journal is enabled
    private bool openOnce = false;
    private int count = 0;

    public string[] objectiveList = new string[] { };
    private CameraScript camerascript;
    private pause pause1;

    public FirstPersonController me;
    // Use this for initialization
    void Start()
    {

        //journalot.enabled = true;
        StartCoroutine(OpenJournalAtStart());

        journal1.enabled = false;

        Clue00.enabled = false;
        Clue01.enabled = false;
        Clue02.enabled = false;
        Clue03_04.enabled = false;
        Clue05.enabled = false;
        Clue06.enabled = false;
        Clue07.enabled = false;
        Clue08.enabled = false;
        Clue09.enabled = false;
        Clue10.enabled = false;


        me = GameObject.FindObjectOfType<FirstPersonController>();
        camerascript = GetComponent<CameraScript>();
        pause1 = GetComponent<pause>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && pause1.checkUI == false)
        {
            journalot.enabled = false;

            journal1.enabled = true;

            journEnabled = true;
            Time.timeScale = 0;
            me.enabled = false;
            Cursor.visible = true;
            count++;
        }
        if (count >= 2)
        {
            journal1.enabled = false;
            Time.timeScale = 1;
            me.enabled = true;
            Cursor.visible = false;
            journEnabled = false;

            Clue00.enabled = false;
            Clue01.enabled = false;
            Clue02.enabled = false;
            Clue03_04.enabled = false;
            Clue05.enabled = false;
            Clue06.enabled = false;
            Clue07.enabled = false;
            Clue08.enabled = false;
            Clue09.enabled = false;
            Clue10.enabled = false;

            count = 0;
        }


        if (journEnabled == true && Time.timeScale == 0)
        {
            Clue00.enabled = true;
            journal1.enabled = true;
            journalot.enabled = false;

        }

        if (camerascript.objectiveCount >= 1 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue00.enabled = false;
            Clue01.enabled = true;
        }
        if(camerascript.objectiveCount == 1 && Time.timeScale == 1)
        {
            if (openOnce == false)
            {
                StartCoroutine(OpenJournal());
            }
        }

        if (camerascript.objectiveCount >= 2 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue01.enabled = false;
            Clue02.enabled = true;
        }
        if (camerascript.objectiveCount == 2 && Time.timeScale == 1)
        {
            if (openOnce == true)
            {
                StartCoroutine(OpenJournal2());
            }
        }

        if (camerascript.objectiveCount >= 3 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue02.enabled = false;
            Clue03_04.enabled = true;
        }
        if (camerascript.objectiveCount == 3 && Time.timeScale == 1)
        {
            if (openOnce == false)
            {
                StartCoroutine(OpenJournal());
            }
        }

        if (camerascript.objectiveCount >= 5 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue03_04.enabled = false;
            Clue05.enabled = true;
        }
        if (camerascript.objectiveCount == 4 && Time.timeScale == 1)
        {
            if (openOnce == true)
            {
                StartCoroutine(OpenJournal2());
            }
        }

        if (camerascript.objectiveCount >= 6 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue05.enabled = false;
            Clue06.enabled = true;
        }
        if (camerascript.objectiveCount == 5 && Time.timeScale == 1)
        {
            if (openOnce == false)
            {
                StartCoroutine(OpenJournal());
            }
        }

        if (camerascript.objectiveCount >= 7 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue06.enabled = false;
            Clue07.enabled = true;
        }
        if (camerascript.objectiveCount == 6 && Time.timeScale == 1)
        {
            if (openOnce == true)
            {
                StartCoroutine(OpenJournal2());
            }
        }

        if (camerascript.objectiveCount >= 8 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue07.enabled = false;
            Clue08.enabled = true;
        }
        if (camerascript.objectiveCount == 7 && Time.timeScale == 1)
        {
            if (openOnce == false)
            {
                StartCoroutine(OpenJournal());
            }
        }

        if (camerascript.objectiveCount >= 9 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue08.enabled = false;
            Clue09.enabled = true;
        }
        if (camerascript.objectiveCount == 8 && Time.timeScale == 1)
        {
            if (openOnce == true)
            {
                StartCoroutine(OpenJournal2());
            }
        }

        if (camerascript.objectiveCount >= 10 && Time.timeScale == 0 && journEnabled == true)
        {
            Clue09.enabled = false;
            Clue10.enabled = true;
        }
        if (camerascript.objectiveCount == 9 && Time.timeScale == 1)
        {
            if (openOnce == false)
            {
                StartCoroutine(OpenJournal());
            }
        }


    }


    IEnumerator OpenJournalAtStart()
    {
        yield return new WaitForSeconds(10);
        journalot.enabled = true;
    }


    IEnumerator OpenJournal()
    {
        yield return new WaitForSeconds(3);
        journalot.enabled = true;
        yield return new WaitForSeconds(1);
        journalot.enabled = false;
        openOnce = true;
    }

    IEnumerator OpenJournal2()
    {
        yield return new WaitForSeconds(3);
        journalot.enabled = true;
        yield return new WaitForSeconds(1);
        journalot.enabled = false;
        openOnce = false;
    }
}
