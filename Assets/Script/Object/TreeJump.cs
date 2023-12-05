using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TreeJump : MonoBehaviourPunCallbacks
{
    public float simultaneousJumpForce = 2.0f; // 동시에 높게 점프할 때 추가로 가해질 힘

    void Update()
    {
        if (photonView.IsMine)
        {
            // 플레이어 점프
            if (Input.GetButtonDown("Jump"))
            {
                photonView.RPC("Jump", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Jump()
    {
        // 플레이어 점프 힘 적용
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * simultaneousJumpForce, ForceMode.Impulse);
    }
}
