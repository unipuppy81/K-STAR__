using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownHillLog : MonoBehaviour
{
    public float downSpeed = 3.0f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (downSpeed - 1) * Time.deltaTime;
            Debug.Log("현재 속도: " + rb.velocity.magnitude);
        }
        //y축이 0보다 크면 속도를 빠르게
       
    }
}
