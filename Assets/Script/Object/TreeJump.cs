using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TreeJump : MonoBehaviourPunCallbacks
{
    public float simultaneousJumpForce = 2.0f; // ���ÿ� ���� ������ �� �߰��� ������ ��

    void Update()
    {
        if (photonView.IsMine)
        {
            // �÷��̾� ����
            if (Input.GetButtonDown("Jump"))
            {
                photonView.RPC("Jump", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Jump()
    {
        // �÷��̾� ���� �� ����
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * simultaneousJumpForce, ForceMode.Impulse);
    }
}
