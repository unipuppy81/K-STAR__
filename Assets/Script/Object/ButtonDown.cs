using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonDown : MonoBehaviour
{
    private PhotonView photonView;

    Animator animator;
    public bool isDown = false;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
       animator = GetComponent<Animator>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �±׸� ã��");
           
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);
           // animator.SetBool("IsDown", true);
            
            Debug.Log("RPC ȣ��");


            /* if (Input.GetButtonDown("Ctrl"))
             {
                 photonView.RPC("SyncPlatformState", RpcTarget.All, true);
                 Debug.Log("��Ʈ�� ���� ");
             }*/
        }
    }

    [PunRPC]
    public void SyncPlatformState(bool isActive)
    {
        // �ִϸ��̼� ����

        if (animator != null)
        {
            Debug.Log("1");
            // �ִϸ��̼� ���� �ڵ� (����: "PressButton" Ʈ���Ÿ� �����ϰ� ����)
            animator.SetTrigger("IsTrigger");
            Debug.Log("2");
        }
    }

    
    void Update()
    {
        // �ٸ� ������ �ʿ��ϴٸ� ���⿡ �߰�
    }
}
