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

    [Header("ï¿½Ìµï¿½ ï¿½ï¿½ È¸ï¿½ï¿½ï¿½Óµï¿½")]
    public float moveSpeed = 8.0f;
    public float turnSpeed = 0.0f;
    public float jumpPower = 5.0f;
    public float jumpPower2 = 30.0f;

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
        /*if(Input.GetKeyDown("space"))
        {
<<<<<<< HEAD
            //if(Physics.Raycast(transform.position,-transform.up,out hit, 1.0f)) // 0.6f¿¡¼­ 1.0f·Î º¯°æ
            Debug.Log("½ºÆäÀÌ½º¹Ù Ãâ·Â");
            if (Physics.Raycast(transform.position,-transform.up,out hit, 1.0f))
=======
            Debug.Log("ì í”„ ì™„ ");
            if (Physics.Raycast(transform.position,-transform.up,out hit, 0.6f))
>>>>>>> 660834ebeb5793ca225e037d8e1ea45575c09078
            {
                Debug.Log("·¹ÀÌÄÉ½ºÆ® Á¡ÇÁ");
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                   
            }
        }*/
    }

    void FixedUpdate()
    {
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.Self);
        transform.Rotate(Vector3.up * Time.smoothDeltaTime * turnSpeed * r);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name==("congconge"))
        {
            rigidbody.AddForce(Vector3.up * jumpPower2, ForceMode.Impulse);
        }
    }
}
