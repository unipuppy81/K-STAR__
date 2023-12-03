using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Cart : MonoBehaviourPunCallbacks
{
    public Transform targetPosition; // 이동할 목표 지점
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.1f; // 멈추기 위한 최소 거리
    private bool isRiding = false; // 탑승 상태 여부
    private GameObject passenger; // 탑승한 플레이어

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F) && CanBoard())
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
                Move();
            }
        }
    }

    private bool CanBoard()
    {
        // 플레이어와 오브젝트 간의 거리가 일정 범위 내에 있는지 확인
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void TryToRide()
    {
        if (!isRiding && CanBoard())
        {
            photonView.RPC("RPC_TryToRide", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_TryToRide(PhotonMessageInfo info)
    {
        // 탑승 로직을 MasterClient에서 실행
        if (!isRiding)
        {
            isRiding = true;
            photonView.TransferOwnership(info.Sender);

            // 클라이언트들에게 탑승 상태 전달
            photonView.RPC("RPC_SetRiding", RpcTarget.All);
        }
    }

    private void ExitRide()
    {
        if (isRiding)
        {
            photonView.RPC("RPC_ExitRide", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_ExitRide()
    {
        // 하차 로직을 MasterClient에서 실행
        isRiding = false;

        if (passenger != null)
        {
            // 플레이어의 부모를 초기화
            passenger.transform.parent = null;
            passenger = null;
        }

        photonView.TransferOwnership(0); // 오브젝트 소유권을 다시 초기화

        // 클라이언트들에게 하차 상태 전달
        photonView.RPC("RPC_SetExiting", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SetRiding()
    {
        // 탑승 상태를 설정
        if (passenger != null)
        {
            // 플레이어를 오브젝트의 자식으로 설정
            passenger.transform.parent = transform;
        }
    }

    [PunRPC]
    private void RPC_SetExiting()
    {
        // 하차 상태를 설정
        if (passenger != null)
        {
            // 플레이어의 부모를 초기화
            passenger.transform.parent = null;
        }
    }

    private void Move()
    {
        // 목표 지점까지의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);

        // 일정 거리 이상이면 이동
        if (distanceToTarget > stoppingDistance)
        {
            // 정해진 속도로 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            passenger = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            passenger = null;
        }
    }
}
