using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }
}