// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class improvedPatrol : MonoBehaviour
{

    public Transform[] points; //points that ai patrol
    public Transform playerPoint; //where ai follows
    public BoxCollider boxCollision;
    public Transform toPlayersDeath; //where ai will take player when caught
    public Transform[] toPlayersDeath2; //where ai will take player when caught
    public Transform AIBack; //where player is held when captured
    public FirstPersonController me; //player controller
    private NavMeshAgent agent; //ai

    private int destPoint = 0;
    private int destPoint2 = 0;
    public float distanceAICanAttack = 10.5f;
    public float AiAddSpeed = .5f;

    private bool check = false;
    private bool isCaught = false;
    private bool isInVisionBox = false;
    private bool doOnce = false;

    public float heightMultiplier = 1.75f;
    public float sightDistance = 50;
    public Vector3 angle;
    private int greatestNum;
    public Transform lastPosition;
    public Transform firstPosition;
    public Transform thisAgent;
    public float runAwayDistance = 40;

    public AudioSource scarySound;
    public Image damaged;
    public Image blackScreen;
    public Image pressToEscapeTxt;
    public float alph = 0;
    public float fadeAmountTime = 1;


    private int randNum = 10;
    private int timesPressed = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        thisAgent = GetComponent<Transform>();
        damaged.enabled = false;
        blackScreen.enabled = false;
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        if (angle.z == 0)
        {
            angle.z = 0.3f;
        }

        GotoNextPoint();

        if (toPlayersDeath2.Length != 0) { 
            greatestNum = toPlayersDeath2.Length - 1;
        }
        
        if(lastPosition == null)
        {
            lastPosition = toPlayersDeath2[greatestNum];
        }

        if(firstPosition == null)
        {
            firstPosition = toPlayersDeath2[0];
        }
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;

    }

    void GotoNextPoint2()
    {
        // Returns if no points have been set up
        if (toPlayersDeath2.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = toPlayersDeath2[destPoint2].position;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint2 = (destPoint2 + 1) % toPlayersDeath2.Length;
        Debug.Log(destPoint2);
    }

    int ClosestPoint()
    {
        int closestPoint = 0;
        for (int i = 0; i < toPlayersDeath2.Length; i++)
        {
            if (Vector3.Distance(toPlayersDeath2[destPoint2].position, agent.transform.position) > Vector3.Distance(toPlayersDeath2[i].position, agent.transform.position))
            {
                
                //Debug.Log("DESTPOINT2:" + destPoint2 + Vector3.Distance(toPlayersDeath2[destPoint2].position, agent.transform.position) + "is GREATER THAN I:" + i +(Vector3.Distance(toPlayersDeath2[i].position, agent.transform.position) ));
                closestPoint = i;
            }


        }
        return closestPoint;
    }
    
        
    

    void Update()
    {


        RaycastHit hit;
        
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward) * sightDistance, Color.green);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right * angle.z).normalized * sightDistance, Color.green);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right * angle.z).normalized * sightDistance, Color.green);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 2 * angle.z).normalized * sightDistance, Color.blue);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 2 * angle.z).normalized * sightDistance, Color.blue);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 1.25f * angle.z).normalized * sightDistance, Color.cyan);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 1.25f * angle.z).normalized * sightDistance, Color.cyan);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 4 * angle.z).normalized * sightDistance, Color.gray);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 4 * angle.z).normalized * sightDistance, Color.grey);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 6 * angle.z).normalized * sightDistance, Color.red);
        Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 6 * angle.z).normalized * sightDistance, Color.red);

    

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward), out hit, sightDistance))//forward
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward) * sightDistance, Color.green);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right * angle.z).normalized, out hit, sightDistance))//left
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right * angle.z).normalized * sightDistance, Color.green);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right * angle.z).normalized, out hit, sightDistance))//right
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right * angle.z).normalized * sightDistance, Color.green);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 2 * angle.z).normalized, out hit, sightDistance))//left
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 2 * angle.z).normalized * sightDistance, Color.blue);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 2 * angle.z).normalized, out hit, sightDistance))//right
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 2 * angle.z).normalized * sightDistance, Color.blue);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 1.25f * angle.z).normalized, out hit, sightDistance))//left
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 1.25f * angle.z).normalized * sightDistance, Color.cyan);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 1.25f * angle.z).normalized, out hit, sightDistance))//right
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 1.25f * angle.z).normalized * sightDistance, Color.cyan);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 4 * angle.z).normalized, out hit, sightDistance))//left
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 4 * angle.z).normalized * sightDistance, Color.gray);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 4 * angle.z).normalized, out hit, sightDistance))//right
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 4 * angle.z).normalized * sightDistance, Color.gray);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 6 * angle.z).normalized, out hit, sightDistance))//left
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward + agent.transform.right / 6 * angle.z).normalized * sightDistance, Color.red);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }

        if (Physics.Raycast(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 6 * angle.z).normalized, out hit, sightDistance))//right
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(agent.transform.position + Vector3.up * heightMultiplier, (agent.transform.forward - agent.transform.right / 6 * angle.z).normalized * sightDistance, Color.red);
                check = true;
                agent.destination = playerPoint.transform.position;
                Debug.Log("It works(drawRay)AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (doOnce == false)
                {
                    agent.speed += AiAddSpeed;
                    doOnce = true;
                }
                else
                {
                    if (Vector3.Distance(me.transform.position, thisAgent.transform.position) > runAwayDistance)
                    {
                        //Debug.Log(Vector3.Distance(me.transform.position, thisAgent.transform.position));
                        //Debug.Log((me.transform.position).magnitude);
                        check = false;
                        GotoNextPoint();
                    }
                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Debug.Log(destPoint2);
        Debug.Log(ClosestPoint() + " is the closest point" );
        // Choose the next destination point when the agent gets
        // close to the current one.

        if (agent.destination != points[destPoint].position) //if destination is not equal to current point
        {
            if (check == true) //check to see if the player went into AI vision box
            {
                if (isCaught == false)
                {
                    agent.destination = playerPoint.transform.position; //make the AI follow the player
                    //Debug.Log("isCaught==false");
                }
                if (Vector3.Distance(playerPoint.transform.position, agent.transform.position) < distanceAICanAttack && agent.remainingDistance < 0.5f ) //check distance to activate attack, must use Distance method for approximation
                {
                    damaged.enabled = true;
                    blackScreen.enabled = true;
                    pressToEscapeTxt.enabled = true;
                    isCaught = true;

                        if (agent.destination != toPlayersDeath2[greatestNum].position)
                        {
                            //agent.destination = toPlayersDeath2[ClosestPoint()].position; 
                            GotoNextPoint2();

                        }

                        if (Vector3.Distance(toPlayersDeath2[greatestNum].position, agent.transform.position) < 1.5f || agent.transform.position == lastPosition.transform.position)//reached death point
                        {
                            damaged.enabled = false;
                            blackScreen.enabled = false;
                            pressToEscapeTxt.enabled = false;
                            scarySound.Stop();

                            isCaught = false;
                            check = false;
                        //agent.transform.position = points[0].position;
                            agent.transform.position = firstPosition.transform.position;
                            //agent.transform.position = firstPosition.transform.position;
                            scarySound.Stop();
                            Debug.Log("Player reached death point.");
                        }
                    if(toPlayersDeath2 == null)
                    {
                        agent.destination = toPlayersDeath.transform.position;
                        Debug.Log("toPlayersDeath2 == null");
                    }

                    if (Vector3.Distance(toPlayersDeath.transform.position, agent.transform.position) < 1.5f)//reached death point
                    {
                        isCaught = false;
                        scarySound.Stop();

                        Debug.Log("isCaught==falseAGAIN");

                    }
                    //Debug.Log("Should work");

                }//main loop that says if the player is within a certain distance, they will be caught
                //Debug.Log("check = false");
            }//loop checking if the player is in the sound/vision box

        }//main loop to move the ai from point to point


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        if (isCaught == true)
        {
            agent.speed += AiAddSpeed;
            me.transform.position = AIBack.position; // moves the player to the AI location
                                                     //damaged.enabled = true;
            if (Time.timeScale == 1)
            {
                blackScreen.canvasRenderer.SetAlpha(alph);
                alph += (fadeAmountTime / 1000);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
               timesPressed += 1;
            }
            randNum = (Random.Range(15, 35));
            //Debug.Log(randNum + " randNum");
        }

        if (alph >= 1 )
        {
            agent.transform.position = lastPosition.transform.position;
            damaged.enabled = false;
            blackScreen.enabled = false;
            pressToEscapeTxt.enabled = false;
            scarySound.Stop();

            isCaught = false;
            check = false;
            //agent.transform.position = points[1].position;
            //scarySound.Stop();
            me.transform.position = lastPosition.transform.position;
            agent.transform.position = firstPosition.transform.position;

        }

        if (timesPressed >= randNum)
        {
            damaged.enabled = false;
            blackScreen.enabled = false;
            pressToEscapeTxt.enabled = false;
            scarySound.Stop();

            isCaught = false;
            check = false;
            timesPressed = 0;

        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }

        if (agent.destination == points[destPoint].position) // go to next point
        {
            GotoNextPoint();
        }

        if ( agent.destination == toPlayersDeath2[destPoint2].position && toPlayersDeath2.Length != 0 && toPlayersDeath2 != null)   
        {
            GotoNextPoint2();
        }

        if (agent.speed >= 5f)
        {
            agent.speed = 5f;
        }

        if (agent.pathPending)
        {
            //Debug.Log("Path Pending");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }

    void OnTriggerEnter(Collider other)
    {

            if (other.gameObject.tag == "Player" || check == true)
            {
                check = true;//went into vision box
                isInVisionBox = true;
            if (doOnce == false)
            {
                agent.speed += .5f;
                scarySound.Play();
                doOnce = true;
            }
                Debug.Log("HitsTrue");
            }
        
    }


    
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player" || check == false)
        {
            //check = false;
            //isInVisionBox = false;
            if (doOnce == true)
            {
                agent.speed -= .5f;
                doOnce = false;
                alph = 0;
            }
            scarySound.Stop();

            //agent.destination = points[destPoint].position;
            Debug.Log("HitsFalse");
        }
    }
    

}