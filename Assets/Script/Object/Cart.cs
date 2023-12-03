using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Cart : MonoBehaviourPunCallbacks
{
    public Transform targetPosition; // �̵��� ��ǥ ����
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.1f; // ���߱� ���� �ּ� �Ÿ�
    private bool isRiding = false; // ž�� ���� ����
    private GameObject passenger; // ž���� �÷��̾�

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
        // �÷��̾�� ������Ʈ ���� �Ÿ��� ���� ���� ���� �ִ��� Ȯ��
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
        // ž�� ������ MasterClient���� ����
        if (!isRiding)
        {
            isRiding = true;
            photonView.TransferOwnership(info.Sender);

            // Ŭ���̾�Ʈ�鿡�� ž�� ���� ����
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
        // ���� ������ MasterClient���� ����
        isRiding = false;

        if (passenger != null)
        {
            // �÷��̾��� �θ� �ʱ�ȭ
            passenger.transform.parent = null;
            passenger = null;
        }

        photonView.TransferOwnership(0); // ������Ʈ �������� �ٽ� �ʱ�ȭ

        // Ŭ���̾�Ʈ�鿡�� ���� ���� ����
        photonView.RPC("RPC_SetExiting", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SetRiding()
    {
        // ž�� ���¸� ����
        if (passenger != null)
        {
            // �÷��̾ ������Ʈ�� �ڽ����� ����
            passenger.transform.parent = transform;
        }
    }

    [PunRPC]
    private void RPC_SetExiting()
    {
        // ���� ���¸� ����
        if (passenger != null)
        {
            // �÷��̾��� �θ� �ʱ�ȭ
            passenger.transform.parent = null;
        }
    }

    private void Move()
    {
        // ��ǥ ���������� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);

        // ���� �Ÿ� �̻��̸� �̵�
        if (distanceToTarget > stoppingDistance)
        {
            // ������ �ӵ��� �̵�
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
