using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/First Person Controller")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonShooterController : MonoBehaviour
{
    [Header("Speed Adjustment")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 1f;
    public float jumpHeight = 5f;
    public float jumpAllowance = 1f;
    public float dashSpeedMultiplier = 2.5f;
    public float dashTime = .25f;
    public float sprintSpeedMultiplier = 1.5f;
    public float headBobHeight = 1f;
    public float strafeAirControlDivisor = 1.25f;
    [Tooltip("The lower, the faster the bob")]
    public float bobSpeed = .5f;

    [Header("Movement Setup")]
    public string[] moveLeft = { "a", "left" };
    public string[] moveRight = { "d", "right" };
    public string[] moveUp = { "w", "up" };
    public string[] moveDown = { "s", "down" };
    public string[] jump = { "space" };
    public string[] dash = { "mouse 1", "left ctrl" };
    public string[] sprint = { "left shift" };
    public string[] shoot = { "mouse 0" };
    [Header("Allowances")]
    public bool allowDash = true;
    public bool allowHeadBob = true;
    public bool dashForwardOnly = true;
    public bool dashInAir = true;
    public bool breathingEffect = true;
    public bool allowFullAuto = true;
    public bool holdToRun = false;
    public int numberOfJumps = 1;
    public float dashDistanceAllowance = 10f;
    [Header("Look Angles")]
    public float minimumYLook = -60F;
    public float maximumYLook = 60F;
    [Header("Projectile Refinement")]
    public GameObject projectile;
    public float projectileSpeed = 25f;
    public float fireRate = .1f;
    public float bulletContactLimit = 1f;
    public float destroyProjectileTime = 1f;
    public AudioClip shootSound;
    [Header("Game Object Children (Put children in same order)")]
    public Camera mycam;
    public Transform dasher;

    private float xAxis;
    private float yAxis;
    private float zAxis;
    private Rigidbody rb;
    private float rotationY = 0F;
    private bool isGrounded = true;
    private Collider myCol;
    private float distanceToGround;
    private float originalSpeed;
    private int jumpsNum = 1;
    private float elapsed = 0;
    private bool shotOnce = false;
    private bool isRunning = false;
    private int runCount = 0;
    private float newSprintSpeed;
    private List<GameObject> newProjArr = new List<GameObject>();
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        mycam = GetComponentInChildren<Camera>();
        myCol = GetComponent<Collider>();
        dasher = transform.GetChild(1).GetComponent<Transform>();
        originalSpeed = moveSpeed;
        newSprintSpeed = moveSpeed * sprintSpeedMultiplier;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            //right
            if (getInput(moveRight, false))
            {
                if (dashForwardOnly == true)
                {
                    if(jumpsNum == 1)
                        transform.position += Time.deltaTime * moveSpeed * dasher.transform.right;
                    if(jumpsNum != 1)
                        transform.position += Time.deltaTime * moveSpeed / strafeAirControlDivisor * dasher.transform.right;
                }
                if (dashForwardOnly == false)
                {
                    if (jumpsNum == 1)
                        transform.position += Time.deltaTime * moveSpeed * mycam.transform.right;
                    if (jumpsNum != 1)
                        transform.position += Time.deltaTime * moveSpeed / strafeAirControlDivisor * mycam.transform.right;
                }
            }

            //left
            if (getInput(moveLeft, false))
            {
                if (dashForwardOnly == true)
                {
                    if (jumpsNum == 1)
                        transform.position += Time.deltaTime * moveSpeed * -dasher.transform.right;
                    if (jumpsNum != 1)
                        transform.position += Time.deltaTime * moveSpeed / strafeAirControlDivisor * -dasher.transform.right;

                }
                if (dashForwardOnly == false)
                {
                    if (jumpsNum == 1)
                        transform.position += Time.deltaTime * moveSpeed * -mycam.transform.right;
                    if (jumpsNum != 1)
                        transform.position += Time.deltaTime * moveSpeed / strafeAirControlDivisor * -mycam.transform.right;
                }
            }

            //forward
            if (getInput(moveUp, false))
            {
                if (dashForwardOnly == true)
                {
                    transform.position += Time.deltaTime * moveSpeed * dasher.transform.forward;
                }
                if (dashForwardOnly == false)
                {
                    transform.position += Time.deltaTime * moveSpeed * mycam.transform.forward;
                }

                //dash with forward only
                //enables dashing in air
                if (getInput(dash, true) && allowDash == true && dashForwardOnly == true && dashInAir == true)
                {
                    StartCoroutine(dashForTime());
                }
                //no dashing in air
                if (getInput(dash, true) && allowDash == true && dashForwardOnly == true && jumpsNum == 1 && dashInAir == false)
                {
                    StartCoroutine(dashForTime());
                }
            }

            //back
            if (getInput(moveDown, false))
            {
                if (dashForwardOnly == true)
                {
                    transform.position += Time.deltaTime * moveSpeed * -dasher.transform.forward;
                }
                if (dashForwardOnly == false)
                {
                    transform.position += Time.deltaTime * moveSpeed * -mycam.transform.forward;
                }
            }
            if (holdToRun == true)
            {
                //run
                if (getInput(sprint, false))
                {
                    //transform.position += Time.deltaTime * moveSpeed * sprintSpeed * dasher.transform.forward;
                    print("Running");
                    moveSpeed = newSprintSpeed;
                }
                if (!getInput(sprint, false))
                {
                    //moveSpeed = originalSpeed;
                }
            }
            if (holdToRun == false)
            {
                //run
                if (getInput(sprint, true))
                {
                    //press once
                    if (runCount == 0)
                    {
                        isRunning = true;
                    }
                    //press twice to turn off
                    if (runCount > 0)
                    {
                        isRunning = false;
                        runCount = 0;
                    }
                    if (isRunning == true)
                    {
                        runCount++;
                    }
                }
            }

            if (allowFullAuto == false)
            {
                //shoot once
                if (getInput(shoot, true))
                {
                    shootProjectile();
                }
            }
            if (allowFullAuto == true)
            {
                //shoot auto
                if (getInput(shoot, false))
                {
                    if (shotOnce == false)
                    {
                        StartCoroutine(fullAutoShoot());
                    }
                }
            }

            //dash without Forward
            //enables dashing in air
            if (getInput(dash, true) && allowDash == true && dashForwardOnly == false && dashInAir == true)
            {
                StartCoroutine(dashForTime());
            }
            //no dashing in air
            if (getInput(dash, true) && allowDash == true && dashForwardOnly == false && jumpsNum == 1 && dashInAir == false)
            {
                StartCoroutine(dashForTime());
            }

            //bob head when moving, unless isjumping or is disabled... Need to fix
            if (isGrounded && allowHeadBob == true && !getInput(dash, false) && !getInput(shoot, false) && !getInput(shoot, true) && jumpsNum == 1)
            {
                elapsed += Time.deltaTime;
                //maybe take away the or part.. not sure
                if (elapsed < bobSpeed)
                {
                    mycam.transform.localPosition += new Vector3(0, Time.deltaTime * headBobHeight, 0);
                }
                if (elapsed > bobSpeed)
                {
                    mycam.transform.localPosition -= new Vector3(0, Time.deltaTime * headBobHeight, 0);

                    if (elapsed > bobSpeed * 1.85f)
                    {
                        elapsed = 0;
                    }
                }
            }
        }
        float rotationX = mycam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumYLook, maximumYLook);
        mycam.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        dasher.transform.localEulerAngles = new Vector3(0, rotationX, 0);
        
        //breathing effect
        if (breathingEffect)
        {
            mycam.transform.position += new Vector3(0, Mathf.Sin(Time.fixedTime) / 10 * Time.fixedDeltaTime, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpsNum += 1;
        }

        if (isRunning == true && Input.anyKey)
        {
            //transform.position += Time.deltaTime * moveSpeed * sprintSpeed * dasher.transform.forward;
            moveSpeed = newSprintSpeed;
        }
        if (isRunning == false && !getInput(dash, false) && !getInput(sprint, false))
        {
            moveSpeed = originalSpeed;
        }
    }

    private void FixedUpdate()
    {
        distanceToGround = myCol.bounds.extents.y;
        RaycastHit hit;
        bool checkGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround + jumpAllowance);
        if (checkGrounded)
        {
            isGrounded = true;
            jumpsNum = 1;
        }

        //jump
        if (getInput(jump, true))
        {
            if (numberOfJumps != 1)
            {
                if (isGrounded == true)
                {
                    rb.velocity = Vector3.up * jumpHeight * Time.deltaTime * 10 * -Physics.gravity.y;
                    //if we havent reaches X number of jumps, allow keep jumping
                    if (jumpsNum >= numberOfJumps)
                    {
                        isGrounded = false;
                    }
                }
            }
            else
            {
                if (isGrounded == true && jumpsNum == 1)
                {
                    rb.velocity = Vector3.up * jumpHeight * Time.deltaTime * 10 * -Physics.gravity.y;
                    isGrounded = false;
                    jumpsNum++;
                }
                jumpsNum = 1;
            }
        }
        Debug.DrawRay(mycam.transform.position, mycam.transform.forward * dashDistanceAllowance, Color.red);
        Debug.DrawRay(dasher.transform.position, dasher.transform.forward * dashDistanceAllowance, Color.blue);
        Debug.DrawRay(transform.position, -Vector3.up * (jumpAllowance + distanceToGround), Color.yellow);
        if (newProjArr != null)
        {
            for (int i = 0; i < newProjArr.Count; i++)
            {
                drawBulletRay(newProjArr[i].transform);
            }
            //drawRay(newProj.transform);
        }
    }

    bool getInput(string[] usedKeys, bool getKeyDown = true)
    {
        string usedKey = usedKeys[0];
        foreach (string i in usedKeys)
        {
            if (Input.GetKey(i))
            {
                usedKey = i;
            }
        }
        if (getKeyDown)
        {
            return Input.GetKeyDown(usedKey);
        }
        else
        {
            return Input.GetKey(usedKey);
        }
    }

    void shootProjectile()
    {
        //GameObject newProjectile = Instantiate(projectile, transform);
        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = projectile.transform.position;
        newProjectile.transform.forward = mycam.transform.forward;
        newProjectile.SetActive(true);
        Rigidbody projectileRB = newProjectile.GetComponent<Rigidbody>();
        projectileRB.velocity += mycam.transform.forward * projectileSpeed * 100 * Time.deltaTime;
        if (newProjectile != null)
        {
            newProjArr.Add(newProjectile);
        }
        if (shootSound != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = shootSound;
            audio.PlayOneShot(shootSound);
        }
        projectileRB.constraints = RigidbodyConstraints.FreezeRotationX;
        projectileRB.constraints = RigidbodyConstraints.FreezeRotationY;
        projectileRB.AddForce(mycam.transform.forward * projectileSpeed * 100);
        StartCoroutine(deleteProjectile(newProjectile));
    }

    void drawBulletRay(Transform projectile)
    {
        Debug.DrawRay(projectile.transform.position, projectile.transform.forward * bulletContactLimit, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
    }

    IEnumerator fullAutoShoot()
    {
        shootProjectile();
        shotOnce = true;
        yield return new WaitForSeconds(fireRate);
        shotOnce = false;
    }


    IEnumerator deleteProjectile(GameObject p)
    {
        yield return new WaitForSeconds(destroyProjectileTime);
        if (p != null)
        {
            newProjArr.Remove(p);
        }
        Destroy(p);
    }

    IEnumerator dashForTime()
    {
        RaycastHit hit;
        //if (!Physics.Raycast(mycam.transform.position, mycam.transform.forward, out hit, dashDistanceAllowance))
        if (!Physics.Raycast(dasher.transform.position, dasher.transform.forward, out hit, dashDistanceAllowance))
        {
            moveSpeed *= dashSpeedMultiplier;
            yield return new WaitForSeconds(dashTime);
            moveSpeed = originalSpeed;
        }
    }
}