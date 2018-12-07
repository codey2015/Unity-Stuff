using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bobber : MonoBehaviour {

    public float speed = 2f;
    public float newBaseOffset = 3f;

    public float slowAt = .15f;
    [Range(0f, 1f)]
    public float slowByPercent = .5f;

    public Transform scalableObject;
    public Vector3 scaleValues = new Vector3(1f, 1.5f, 1f);
    [SerializeField]
    private Vector3 originalScale = new Vector3(0f, 0f, 0f);
    public bool onlyScale = false;
    public float scaleSpeed = 2f;
    public float scalableWaitTime = 1f;

    [Header("Turns off audio source by default")]
    public AudioSource footSteps;
    [Range(0, 0.5f)]
    public float pitchRandomize;
    float originalPitch;
    public int limitFootSteps = 5;
    public static int countFootStepNumber = 0;
    public int seeNumberOfFootSteps;

    private NavMeshAgent agent;
    private float startBaseOffset;
    private float originalSpeed;
    private float elapsed = 0f;
    private float close;
    private bool isGreater = false;
    private bool slowed = false;
    private bool goingUp = true;
    private bool hasABaseOffset = true;
    private bool checkPlayingOnce = false;
    private Transform originalTransform;
    // Use this for initialization
    void Start () {
		

        agent = GetComponent<NavMeshAgent>();
        startBaseOffset = agent.baseOffset;
        originalSpeed = speed;

        if(scalableObject == null)
        {
            scalableObject = gameObject.transform;
        }
        originalScale = scalableObject.transform.localScale;

        scaleValues /= 100;
        originalTransform = scalableObject;

        if(newBaseOffset == 0 || speed == 0)
        {
            hasABaseOffset = false;
        }

        if (footSteps == null)
        {
            footSteps = gameObject.GetComponent<AudioSource>();
        }
        if (footSteps != null)
        {
            footSteps.Stop();
            originalPitch = footSteps.pitch;
        }

        gameObject.transform.GetComponent<AudioSource>().enabled = false;
        //countFootStepNumber++;

        if (countFootStepNumber < limitFootSteps)
        {
            this.gameObject.transform.GetComponent<AudioSource>().enabled = true;
            countFootStepNumber++;
        }
    }
	
	void Update () {
        if (onlyScale == false)
        {
            moveMyAgent();
        }
        if(onlyScale == true)
        {
            squashAndStretch();
        }
        //if countFootStepNumber is <= limit, then get their audio comonent and set to active and vice versa.
        seeNumberOfFootSteps = countFootStepNumber;
    }

    void moveMyAgent()
    {
        if (isGreater == false)
        {
            agent.baseOffset += speed * Time.deltaTime;
            scalableObject.localScale = Vector3.Slerp(scalableObject.localScale, scaleValues, scaleSpeed * Time.deltaTime);

            if (Mathf.Abs(agent.baseOffset - newBaseOffset) < slowAt && slowed == false)
            {
                speed *= slowByPercent;
                slowed = true;
            }

            if (agent.baseOffset >= newBaseOffset)
            {
                speed = originalSpeed;
                isGreater = true;
            }

        }

        if (isGreater == true)
        {
            agent.baseOffset -= speed * Time.deltaTime;
            scalableObject.localScale = Vector3.Slerp(originalTransform.localScale, originalScale, scaleSpeed * Time.deltaTime);
            if (Mathf.Abs(agent.baseOffset - startBaseOffset) < slowAt && slowed == true)
            {
                speed *= slowByPercent;
                slowed = false;
            }

            if (agent.baseOffset <= startBaseOffset)
            {
                speed = originalSpeed;
                isGreater = false;
            }
        }
    }

    void squashAndStretch()
    {
        if (goingUp == true)
        {
            scalableObject.localScale = Vector3.Slerp(scalableObject.localScale, scaleValues, scaleSpeed * Time.deltaTime);
            close = Vector3.Distance(scalableObject.localScale, scaleValues);
            if (Mathf.Abs(close) < .003f)
            {
                StartCoroutine(slightWaitUp());
                /*
                if (countFootStepNumber < limitFootSteps)
                {
                    gameObject.transform.GetComponent<AudioSource>().enabled = true;
                    if(checkPlayingOnce == false)
                    {
                        countFootStepNumber++;
                        checkPlayingOnce = true;

                    }
                }
                */
                if (footSteps != null)
                {
                    float newPitch = Random.Range(-pitchRandomize, pitchRandomize) + footSteps.pitch;
                    footSteps.pitch = newPitch;
                    footSteps.Play();
                    StartCoroutine(pitchShift());
                }
            }
        }


        if (goingUp == false)
        {
            scalableObject.localScale = Vector3.Slerp(originalTransform.localScale, originalScale, scaleSpeed * Time.deltaTime);
            close = Vector3.Distance(scalableObject.localScale, originalScale);
            if (Mathf.Abs(close) < .003f)
            {
                StartCoroutine(slightWaitDown());

                if (footSteps != null)
                {
                    float newPitch = Random.Range(-pitchRandomize, pitchRandomize) + footSteps.pitch;
                    footSteps.pitch = newPitch;
                    footSteps.Play();
                    StartCoroutine(pitchShift());
                }
            }
        }

    }
    IEnumerator slightWaitUp()
    {
        yield return new WaitForSeconds(scalableWaitTime);
        goingUp = false;
    }

    IEnumerator slightWaitDown()
    {
        yield return new WaitForSeconds(scalableWaitTime);
        goingUp = true;
    }

    IEnumerator pitchShift()
    {
        yield return new WaitForSeconds(.5f);
        footSteps.pitch = originalPitch;
    }
}
