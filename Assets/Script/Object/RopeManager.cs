using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeManager : MonoBehaviourPunCallbacks
{
    public SpringJoint ropeJoint;
    public Transform connectedPlayer;

    private LineRenderer ropeRenderer;  // LineRenderer 변수 추가

    void Awake()
    {
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

        // LineRenderer 초기화
        InitializeRopeRenderer();
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

        // LineRenderer 업데이트
        UpdateRopeRenderer();
    }

    void InitializeRopeRenderer()
    {
        // LineRenderer 컴포넌트 가져오기 또는 추가하기
        if (!ropeRenderer)
        {
            ropeRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // 라인의 포인트 개수 설정 (시작점, 끝점, 중간 등)
        ropeRenderer.positionCount = 2;

        // 라인 두께 및 다른 속성 설정
        ropeRenderer.startWidth = 0.1f;
        ropeRenderer.endWidth = 0.1f;
        ropeRenderer.material.color = Color.red;
    }

    void UpdateRopeRenderer()
    {
        // LineRenderer의 시작점과 끝점 설정
        if (connectedPlayer)
        {
            ropeRenderer.SetPosition(0, transform.position);
            ropeRenderer.SetPosition(1, connectedPlayer.position);
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