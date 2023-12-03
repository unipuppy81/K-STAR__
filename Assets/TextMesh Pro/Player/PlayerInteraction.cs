using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//카트타기위한 스크립트
public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    public Transform targetDestination;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            // 오브젝트가 이동 중일 때 동기화
            photonView.RPC("MoveObject", RpcTarget.AllBuffered, targetDestination.position);

            // 일정 좌표에 도달하면 이동 중지
            if (Vector3.Distance(transform.position, targetDestination.position) < 0.1f)
            {
                photonView.RPC("StopObject", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void MoveObject(Vector3 destination)
    {
        // RPC를 통해 모든 플레이어가 목표 지점까지 이동한 오브젝트를 동기화
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 5f);
    }

    [PunRPC]
    void StopObject()
    {
        // RPC를 통해 모든 플레이어가 오브젝트의 이동을 중지
        isMoving = false;
    }

    public void StartMoving()
    {
        // 다른 스크립트에서 이 메서드를 호출하여 오브젝트의 이동을 시작
        isMoving = true;
    }
}
