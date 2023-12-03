using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Cart : MonoBehaviourPunCallbacks , IPunObservable
{
    public Transform targetPositionOnTop; // 탑승할 때 이동할 목표 지점(카트 이동 목표 지점)
    public Transform targetPositionBeside; // 하차할 때 이동할 목표 지점(하차시 플레이어 위치)
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 1.0f; // 목표 지점에 도달할 때의 거리
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
                MoveObject();
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
        if (!isRiding)
        {
            isRiding = true;
            photonView.TransferOwnership(info.Sender);

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
        isRiding = false;

        if (passenger != null)
        {
            // 플레이어가 탑승할 때 위치 이동
            passenger.transform.parent = null;
            if (targetPositionBeside != null)
            {
                passenger.transform.position = targetPositionBeside.position;
            }
            passenger = null;
        }

        photonView.TransferOwnership(0);

        photonView.RPC("RPC_SetExiting", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SetRiding()
    {
        if (passenger != null)
        {
            // 플레이어가 탑승할 때 위치 이동
            passenger.transform.parent = transform;
            if (targetPositionOnTop != null)
            {
                // 플레이어를 오브젝트 위로 이동
                passenger.transform.localPosition = Vector3.zero;
            }
        }
    }

    [PunRPC]
    private void RPC_SetExiting()
    {
        if (passenger != null)
        {
            // 플레이어가 하차할 때 위치 이동
            passenger.transform.parent = null;
            if (targetPositionBeside != null)
            {
                passenger.transform.position = targetPositionBeside.position;
            }
        }
    }

    private void MoveObject()
    {
        // 두 플레이어가 모두 탑승한 경우에만 이동
        if (isRiding && passenger != null && photonView.OwnershipTransfer == OwnershipOption.Takeover)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPositionOnTop.position);

            if (distanceToTarget > stoppingDistance)
            {
                // 정해진 속도로 이동
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
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

    #region IPunObservable Implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 다른 플레이어에게 보내기
            stream.SendNext(isRiding);
        }
        else
        {
            // 다른 플레이어로부터 데이터 받기
            isRiding = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
