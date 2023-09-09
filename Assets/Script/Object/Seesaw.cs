using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Seesaw : MonoBehaviour
{
    public float swingForce = 10f; // 시소에서 힘을 가할 강도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) // 예를 들어, 스페이스 바를 눌렀을 때
        {
            // 플레이어가 점프하는 것을 시뮬레이트하기 위해 오브젝트에 힘을 가합니다.
            Vector3 swingDirection = new Vector3(0f, 0f, 0.2f); // 시소에서의 방향으로 힘을 가합니다.
            rb.AddForce(swingDirection * swingForce, ForceMode.Impulse);
        }
    }
}
