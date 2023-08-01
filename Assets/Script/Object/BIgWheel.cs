using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIgWheel : MonoBehaviour
{
    public float rotationSpeed = 10f; // 회전 속도

    // Update is called once per frame
    void Update()
    { 
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
