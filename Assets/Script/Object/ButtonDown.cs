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
            Debug.Log("플레이어 태그를 찾음");
           
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);
           // animator.SetBool("IsDown", true);
            
            Debug.Log("RPC 호출");


            /* if (Input.GetButtonDown("Ctrl"))
             {
                 photonView.RPC("SyncPlatformState", RpcTarget.All, true);
                 Debug.Log("컨트롤 누름 ");
             }*/
        }
    }

    [PunRPC]
    public void SyncPlatformState(bool isActive)
    {
        // 애니메이션 실행

        if (animator != null)
        {
            Debug.Log("1");
            // 애니메이션 실행 코드 (예시: "PressButton" 트리거를 설정하고 실행)
            animator.SetTrigger("IsTrigger");
            Debug.Log("2");
        }
    }

    
    void Update()
    {
        // 다른 로직이 필요하다면 여기에 추가
    }
}
