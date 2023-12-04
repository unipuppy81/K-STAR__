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
        if (connectedPlayer)
        {
            // 두 플레이어 간의 거리를 계산하여 시작점과 끝점을 설정
            Vector3 startPoint = transform.position;
            Vector3 endPoint = connectedPlayer.position;

            ropeRenderer.SetPosition(0, startPoint);
            ropeRenderer.SetPosition(1, endPoint);
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