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
            Debug.Log("플레이어와 닿음");
            photonView.RPC("SyncPlatformStateTrans", RpcTarget.All, transform.position);
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);

            /*
            if (Input.GetButtonDown("F"))
            {
                Debug.Log("F누름");
                photonView.RPC("SyncPlatformState", RpcTarget.All, transform.position);
                Debug.Log("불러옴1");
            }*/
        }
    }

    [PunRPC]
    public void SyncPlatformStateTrans(Vector3 newPosition)
    {
        if (isTrigger)
        {
            // 현재 플레이어가 소유한 오브젝트에만 위치를 변경합니다.
            player.transform.position = newPosition;
            Debug.Log("불러옴");
        }
    }
    public void SyncPlatformState(bool isActive)
    {
        if (animator != null)
        {
            Debug.Log("1");
            // 애니메이션 실행 코드 (예시: "PressButton" 트리거를 설정하고 실행)
            animator.SetTrigger("IsTrigger");
            Debug.Log("2");
        }
    }
}
