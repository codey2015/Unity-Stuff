using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/First Person Controller")]
[RequireComponent(typeof(Rigidbody))]
public class MyCharacterController : MonoBehaviour
{
    [Header("Speed Adjustment")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 1f;
    public float jumpHeight = 5f;
    public float jumpAllowance = 1f;
    public float dashSpeedMultiplier = 2.5f;
    public float dashTime = .25f;
    public float headBobHeight = 1f;
    [Tooltip("The lower, the faster the bob")]
    public float bobSpeed = .5f;

    [Header("Movement Setup")]
    public string[] moveLeft = { "a", "left" };
    public string[] moveRight = { "d", "right" };
    public string[] moveUp = { "w", "up" };
    public string[] moveDown = { "s", "down" };
    public string[] jump = { "space" };
    public string[] dash = { "mouse 1", "left ctrl" };
    [Header("Allowances")]
    public bool allowDash = true;
    public bool allowHeadBob = true;
    public bool dashForwardOnly = true;
    public bool dashInAir = true;
    public bool breathingEffect = true;
    public int numberOfJumps = 1;
    [Header("Look Angles")]
    public float minimumYLook = -60F;
    public float maximumYLook = 60F;
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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mycam = GetComponentInChildren<Camera>();
        myCol = GetComponent<Collider>();
        dasher = transform.GetChild(1).GetComponent<Transform>();
        originalSpeed = moveSpeed;
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
                    transform.position += Time.deltaTime * moveSpeed * dasher.transform.right;
                }
                if (dashForwardOnly == false)
                {
                    transform.position += Time.deltaTime * moveSpeed * mycam.transform.right;
                }
            }

            //left
            if (getInput(moveLeft, false))
            {
                if (dashForwardOnly == true)
                {
                    transform.position += Time.deltaTime * moveSpeed * -dasher.transform.right;
                }
                if (dashForwardOnly == false)
                {
                    transform.position += Time.deltaTime * moveSpeed * -mycam.transform.right;
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
            if (isGrounded && allowHeadBob == true && !getInput(dash, false))
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

    IEnumerator dashForTime()
    {
            moveSpeed *= dashSpeedMultiplier;
            yield return new WaitForSeconds(dashTime);
            moveSpeed = originalSpeed;
    }
}
