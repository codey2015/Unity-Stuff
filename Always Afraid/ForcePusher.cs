using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePusher : MonoBehaviour
{

    public Vector3 boxSize = new Vector3(1, 1, 5);
    public Vector3 boxSizeOffset = new Vector3(0, .5f, 3.25f);
    public string key = "v";

    public bool useCube = true;
    public GameObject cubeChild;
    public Color cubeChildColor = new Color(1, 1, 1, 1);
    private GameObject AOE;
    private float t = 0f;
    private Color colorNegative;
    private Color colorNegativeTemp;
    private Color cubeChildColorTemp;
    [SerializeField]
    private bool isLerping = false;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.color = cubeChildColor;
        if (!useCube)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero + boxSizeOffset, boxSize);
        }
        if (useCube)
        {
            Gizmos.matrix = Matrix4x4.TRS(cubeChild.transform.position, cubeChild.transform.rotation, Vector3.one);
            //Gizmos.DrawCube(cubeChild.transform.position, cubeChild.transform.lossyScale);
            Gizmos.DrawCube(Vector3.zero, cubeChild.transform.lossyScale);
        }
    }

    void Start()
    {
        cubeChildColorTemp = cubeChildColor;
        colorNegative = new Color(1f - cubeChildColor.r, 1f - cubeChildColor.g, 1f - cubeChildColor.b, 1);
        colorNegativeTemp = colorNegative;

        if (!useCube)
        {
            AOE = GameObject.CreatePrimitive(PrimitiveType.Cube);
            AOE.transform.parent = transform;
            AOE.transform.localScale = boxSize;
            AOE.transform.forward = transform.forward;
            AOE.transform.localPosition = boxSizeOffset;
            AOE.SetActive(false);
        }
        if (useCube)
        {
            cubeChild.transform.localEulerAngles = cubeChild.GetComponentInParent<GameObject>().transform.localEulerAngles;
        }

    }
    private void Update()
    {

        t += Time.deltaTime;
        if (isLerping)
        {
            colorNegative = Color.Lerp(colorNegativeTemp, cubeChildColorTemp, t / 12f);
            cubeChildColor = colorNegative;
        }
        if (!isLerping)
        {
            colorNegative = Color.Lerp(cubeChildColorTemp, colorNegativeTemp, t / 6f);
            cubeChildColor = colorNegative;
        }
        if (t < 6f)
        {
            colorNegative = cubeChildColorTemp;
            isLerping = false;
        }
        if (t >= 6f && t < 12f)
        {
            colorNegative = colorNegativeTemp;
            isLerping = true;
            
        }
        if(t >= 12f)
        {
            t = 0f;
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey(key))
        {
            if (useCube)
                GravityPushWithCube();
            if (!useCube)
                GravityPushWithoutCube();
        }
    }

    void GravityPushWithCube()
    {
        foreach (Collider collider in Physics.OverlapBox(cubeChild.transform.position, cubeChild.transform.lossyScale / 2))
        {
            print(collider.name);
        }
    }
    void GravityPushWithoutCube()
    {
        foreach (Collider collider in Physics.OverlapBox(AOE.transform.position, AOE.transform.lossyScale / 2))
        {
            print(collider.name);
        }
    }
}
