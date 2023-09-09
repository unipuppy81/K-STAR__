using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class congcongE : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 400f;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float jumpZoneForce = 2f;

    bool isJumpZone = false;
    Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="congcongE")
        {
            isJumpZone = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
       if(isJumpZone)
        {
            rb.AddForce(new Vector3(0, jumpZoneForce, 0)*jumpForce);
            isJumpZone = false;
        }
       
    }
}
