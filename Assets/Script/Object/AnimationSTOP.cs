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
        // 다른 플레이어와의 충돌을 감지
        if (other.CompareTag("Bullet"))
        {
            // 로컬 플레이어만 애니메이션을 중지
            if (photonView.IsMine)
            {
                photonView.RPC("StopAnimation", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void StopAnimation()
    {
        
        animator.speed = 0f; // 애니메이션 재생 속도를 0으로 설정하여 정지
       
    }
}
