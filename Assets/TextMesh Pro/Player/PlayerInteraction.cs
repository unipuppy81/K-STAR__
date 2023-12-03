using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//īƮŸ������ ��ũ��Ʈ
public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    public Transform targetDestination;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            // ������Ʈ�� �̵� ���� �� ����ȭ
            photonView.RPC("MoveObject", RpcTarget.AllBuffered, targetDestination.position);

            // ���� ��ǥ�� �����ϸ� �̵� ����
            if (Vector3.Distance(transform.position, targetDestination.position) < 0.1f)
            {
                photonView.RPC("StopObject", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void MoveObject(Vector3 destination)
    {
        // RPC�� ���� ��� �÷��̾ ��ǥ �������� �̵��� ������Ʈ�� ����ȭ
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 5f);
    }

    [PunRPC]
    void StopObject()
    {
        // RPC�� ���� ��� �÷��̾ ������Ʈ�� �̵��� ����
        isMoving = false;
    }

    public void StartMoving()
    {
        // �ٸ� ��ũ��Ʈ���� �� �޼��带 ȣ���Ͽ� ������Ʈ�� �̵��� ����
        isMoving = true;
    }
}
