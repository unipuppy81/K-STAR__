using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AnimationSTOP : MonoBehaviourPun
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ٸ� �÷��̾���� �浹�� ����
        if (other.CompareTag("Bullet"))
        {
            // ���� �÷��̾ �ִϸ��̼��� ����
            if (photonView.IsMine)
            {
                photonView.RPC("StopAnimation", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void StopAnimation()
    {
        
        animator.speed = 0f; // �ִϸ��̼� ��� �ӵ��� 0���� �����Ͽ� ����
       
    }
}
