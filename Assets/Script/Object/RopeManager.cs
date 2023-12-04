using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeManager : MonoBehaviourPunCallbacks
{
    public SpringJoint ropeJoint;
    public Transform connectedPlayer;

    private LineRenderer ropeRenderer;

    void Awake()
    {
        ropeRenderer = GetComponent<LineRenderer>();

        // 플레이어 1이나 2에 따라 다른 초기 위치 설정
        if (photonView.IsMine)
        {
            transform.position = new Vector3(-5f, 0f, 0f);
        }
        else
        {
            transform.position = new Vector3(5f, 0f, 0f);
        }

        // 서로 다른 플레이어에게 물리적 연결을 동기화
        photonView.RPC("SyncRopeConnection", RpcTarget.All, ropeJoint.connectedBody ? photonView.ViewID : 0);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // 플레이어의 이동 입력 처리
            float moveInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * moveInput * Time.deltaTime * 5f);

            // 이동 정보를 다른 플레이어에게 동기화
            photonView.RPC("SyncPlayerMovement", RpcTarget.Others, transform.position);
        }

        // RopeRenderer 업데이트
        UpdateRopeRenderer();
    }

    void UpdateRopeRenderer()
    {
        // LineRenderer에 첫 번째 점을 연결 플레이어 위치로 설정
        ropeRenderer.SetPosition(0, transform.position);

        // LineRenderer에 두 번째 점을 연결된 플레이어 위치로 설정
        if (connectedPlayer != null)
        {
            ropeRenderer.SetPosition(1, connectedPlayer.position);
        }
        else
        {
            // 만약 연결된 플레이어가 없다면, 높은 위치로 설정하여 허공에 떠 있는 것처럼 보이게 함
            ropeRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        }
    }

    [PunRPC]
    void SyncPlayerMovement(Vector3 newPosition)
    {
        // 다른 플레이어가 이동한 정보를 받아와 적용
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }

    [PunRPC]
    void SyncRopeConnection(int connectedPlayerID)
    {
        // 다른 플레이어에게 물리적 연결 정보를 받아와 적용
        if (!photonView.IsMine)
        {
            PhotonView connectedView = PhotonView.Find(connectedPlayerID);
            connectedPlayer = connectedView ? connectedView.transform : null;

            if (connectedPlayer)
            {
                ropeJoint.connectedBody = connectedPlayer.GetComponent<Rigidbody>();
            }
        }
    }
}