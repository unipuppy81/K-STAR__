using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
public class PlayetrCTL : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private PhotonView pv;
    private float v;
    private float h;
    private float r;

    [Header("이동 및 회전속도")]
    public float moveSpeed = 8.0f;
    public float turnSpeed = 0.0f;
    public float jumpPower = 5.0f;

    private float turnSpeedValue = 200.0f;

    RaycastHit hit;

    IEnumerator Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.5f);
        if(pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = transform.Find("CamPivot").transform;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        turnSpeed = turnSpeedValue;
    }

    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        r = Input.GetAxis("Mouse X");

        Debug.DrawRay(transform.position, -transform.up * 0.6f, Color.green);
        if(Input.GetKeyDown("space"))
        {
            if(Physics.Raycast(transform.position,-transform.up,out hit, 0.6f))
            {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.Self);
        transform.Rotate(Vector3.up * Time.smoothDeltaTime * turnSpeed * r);
    }
}
