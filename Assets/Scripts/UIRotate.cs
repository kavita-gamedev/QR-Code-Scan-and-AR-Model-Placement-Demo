using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotate : MonoBehaviour
{
    public float rotateSpeed;
    public Vector3 rotationAxis;
    private void Update()
    {
        transform.Rotate(rotationAxis * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
