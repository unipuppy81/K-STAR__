using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Cart : MonoBehaviourPun
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float moveSpeed = 5.0f;
    private float distanceThreshold = 0.1f;
    private bool isRiding = false; // 탑승 상태 여부
    void Update()
    {
        if (!photonView.IsMine)
        {
            return; // 다른 플레이어의 오브젝트에 대한 업데이트를 무시합니다.
        }

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isRiding)
                {
                    ExitRide();
                }
                else
                {
                    TryToRide();
                }
            }

            if (isRiding)
            {
                MoveAlongPath();
            }
        }
    }

    private void TryToRide()
    {
        if (!isRiding)
        {
            photonView.RPC("RPC_TryToRide", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_TryToRide(PhotonMessageInfo info)
    {
        // 탑승 로직을 MasterClient에서 실행
        // 예를 들어, 탑승 가능한지 여부를 확인하고 탑승하도록 설정
        if (!isRiding)
        {
            isRiding = true;
            photonView.TransferOwnership(info.Sender);
        }
    }

    private void ExitRide()
    {
        if (isRiding)
        {
            photonView.RPC("RPC_ExitRide", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_ExitRide()
    {
        // 하차 로직을 MasterClient에서 실행
        isRiding = false;
        photonView.TransferOwnership(0); // 오브젝트 소유권을 다시 초기화
    }

    private void MoveAlongPath()
    {
        // 경로를 따라 이동
        Vector3 currentWaypoint = waypoints[currentWaypointIndex].position;
        if (Vector3.Distance(transform.position, currentWaypoint) < distanceThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
    }
}
