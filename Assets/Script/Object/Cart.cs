using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Cart : MonoBehaviourPunCallbacks , IPunObservable
{
    public Transform targetPositionOnTop; // ž���� �� �̵��� ��ǥ ����(īƮ �̵� ��ǥ ����)
    public Transform targetPositionBeside; // ������ �� �̵��� ��ǥ ����(������ �÷��̾� ��ġ)
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 1.0f; // ��ǥ ������ ������ ���� �Ÿ�
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
                MoveObject();
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
            // �÷��̾ ž���� �� ��ġ �̵�
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
            // �÷��̾ ž���� �� ��ġ �̵�
            passenger.transform.parent = transform;
            if (targetPositionOnTop != null)
            {
                // �÷��̾ ������Ʈ ���� �̵�
                passenger.transform.localPosition = Vector3.zero;
            }
        }
    }

    [PunRPC]
    private void RPC_SetExiting()
    {
        if (passenger != null)
        {
            // �÷��̾ ������ �� ��ġ �̵�
            passenger.transform.parent = null;
            if (targetPositionBeside != null)
            {
                passenger.transform.position = targetPositionBeside.position;
            }
        }
    }

    private void MoveObject()
    {
        // �� �÷��̾ ��� ž���� ��쿡�� �̵�
        if (isRiding && passenger != null && photonView.OwnershipTransfer == OwnershipOption.Takeover)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPositionOnTop.position);

            if (distanceToTarget > stoppingDistance)
            {
                // ������ �ӵ��� �̵�
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
            // �����͸� �ٸ� �÷��̾�� ������
            stream.SendNext(isRiding);
        }
        else
        {
            // �ٸ� �÷��̾�κ��� ������ �ޱ�
            isRiding = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
