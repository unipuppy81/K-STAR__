using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Windmill : MonoBehaviour
{
    private PhotonView photonView;
    public GameObject player;
    Animator animator;
    private bool isTrigger = false;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)

        {
            isTrigger = true;
            Debug.Log("�÷��̾�� ����");
            photonView.RPC("SyncPlatformStateTrans", RpcTarget.All, transform.position);
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);

            /*
            if (Input.GetButtonDown("F"))
            {
                Debug.Log("F����");
                photonView.RPC("SyncPlatformState", RpcTarget.All, transform.position);
                Debug.Log("�ҷ���1");
            }*/
        }
    }

    [PunRPC]
    public void SyncPlatformStateTrans(Vector3 newPosition)
    {
        if (isTrigger)
        {
            // ���� �÷��̾ ������ ������Ʈ���� ��ġ�� �����մϴ�.
            player.transform.position = newPosition;
            Debug.Log("�ҷ���");
        }
    }
    public void SyncPlatformState(bool isActive)
    {
        if (animator != null)
        {
            Debug.Log("1");
            // �ִϸ��̼� ���� �ڵ� (����: "PressButton" Ʈ���Ÿ� �����ϰ� ����)
            animator.SetTrigger("IsTrigger");
            Debug.Log("2");
        }
    }
}
