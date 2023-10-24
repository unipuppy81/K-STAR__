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
    private bool isRiding = false; // ž�� ���� ����
    void Update()
    {
        if (!photonView.IsMine)
        {
            return; // �ٸ� �÷��̾��� ������Ʈ�� ���� ������Ʈ�� �����մϴ�.
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
        // ž�� ������ MasterClient���� ����
        // ���� ���, ž�� �������� ���θ� Ȯ���ϰ� ž���ϵ��� ����
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
        // ���� ������ MasterClient���� ����
        isRiding = false;
        photonView.TransferOwnership(0); // ������Ʈ �������� �ٽ� �ʱ�ȭ
    }

    private void MoveAlongPath()
    {
        // ��θ� ���� �̵�
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
