using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeManager : MonoBehaviourPunCallbacks
{
    public SpringJoint ropeJoint;
    public Transform connectedPlayer;

    private LineRenderer ropeRenderer;

    void Awake()
    {
        ropeRenderer = GetComponent<LineRenderer>();

        // �÷��̾� 1�̳� 2�� ���� �ٸ� �ʱ� ��ġ ����
        if (photonView.IsMine)
        {
            transform.position = new Vector3(-5f, 0f, 0f);
        }
        else
        {
            transform.position = new Vector3(5f, 0f, 0f);
        }

        // ���� �ٸ� �÷��̾�� ������ ������ ����ȭ
        photonView.RPC("SyncRopeConnection", RpcTarget.All, ropeJoint.connectedBody ? photonView.ViewID : 0);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // �÷��̾��� �̵� �Է� ó��
            float moveInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * moveInput * Time.deltaTime * 5f);

            // �̵� ������ �ٸ� �÷��̾�� ����ȭ
            photonView.RPC("SyncPlayerMovement", RpcTarget.Others, transform.position);
        }

        // RopeRenderer ������Ʈ
        UpdateRopeRenderer();
    }

    void UpdateRopeRenderer()
    {
        // LineRenderer�� ù ��° ���� ���� �÷��̾� ��ġ�� ����
        ropeRenderer.SetPosition(0, transform.position);

        // LineRenderer�� �� ��° ���� ����� �÷��̾� ��ġ�� ����
        if (connectedPlayer != null)
        {
            ropeRenderer.SetPosition(1, connectedPlayer.position);
        }
        else
        {
            // ���� ����� �÷��̾ ���ٸ�, ���� ��ġ�� �����Ͽ� ����� �� �ִ� ��ó�� ���̰� ��
            ropeRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        }
    }

    [PunRPC]
    void SyncPlayerMovement(Vector3 newPosition)
    {
        // �ٸ� �÷��̾ �̵��� ������ �޾ƿ� ����
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }

    [PunRPC]
    void SyncRopeConnection(int connectedPlayerID)
    {
        // �ٸ� �÷��̾�� ������ ���� ������ �޾ƿ� ����
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