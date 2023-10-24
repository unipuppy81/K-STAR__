using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//카트타기위한 스크립트
public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    private Cart currentRideable; // 현재 탑승 중인 오브젝트

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentRideable != null)
                {
                    ExitRide();
                }
                else
                {
                    TryToRide();
                }
            }
        }
    }

    private void TryToRide()
    {
        if (currentRideable == null)
        {
            // 레이캐스트를 사용하여 탑승 가능한 오브젝트를 찾습니다.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
            {
                Cart rideable = hit.collider.GetComponent<Cart>();
                if (rideable != null)
                {
                    photonView.RPC("RPC_TryToRide", RpcTarget.MasterClient, rideable.photonView.ViewID);
                }
            }
        }
    }

    [PunRPC]
    private void RPC_TryToRide(int rideableID, PhotonMessageInfo info)
    {
        if (currentRideable == null)
        {
            PhotonView rideableView = PhotonView.Find(rideableID);
            if (rideableView != null)
            {
                Cart rideable = rideableView.GetComponent<Cart>();
                if (rideable != null)
                {
                    currentRideable = rideable;
                    currentRideable.photonView.TransferOwnership(info.Sender);
                    currentRideable.photonView.RPC("RPC_TryToRide", RpcTarget.AllBuffered);
                }
            }
        }
    }

    private void ExitRide()
    {
        if (currentRideable != null)
        {
            photonView.RPC("RPC_ExitRide", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_ExitRide(PhotonMessageInfo info)
    {
        if (currentRideable != null)
        {
            currentRideable.photonView.TransferOwnership(0);
            currentRideable.photonView.RPC("RPC_ExitRide", RpcTarget.AllBuffered);
            currentRideable = null;
        }
    }
}
