using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float springForce = 5f;
    public float dampingRatio = 1f;
    public float moveSpeed = 5f; // 이동 속도

    void Start()
    {
        CreateRope();
    }

    void CreateRope()
    {
        // 각각의 GameObject에 Rigidbody 추가
        //Rigidbody rb1 = object1.AddComponent<Rigidbody>();
        //Rigidbody rb2 = object2.AddComponent<Rigidbody>();

        // Spring Joint 추가
        SpringJoint springJoint = object1.AddComponent<SpringJoint>();
        springJoint.connectedBody = object2.GetComponent<Rigidbody>();
        springJoint.spring = springForce;
        springJoint.damper = dampingRatio;
        springJoint.maxDistance = 10.0f;
        springJoint.minDistance = 1.0f;
    }
}
