using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//카트타기위한 스크립트
public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    private bool isInteracting = false;
    private GameObject interactableObject = null;

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isInteracting)
            {
                // 이미 상호작용 중이라면 탈출
                TryExitObject();
            }
            else
            {
                // 상호작용 시작 시도
                TryEnterObject();
            }
        }
    }

    void TryEnterObject()
    {
        // 주변에 있는 InteractableObject를 확인
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("InteractableObject"))
            {
                ObjectInteraction interactableObjectScript = collider.GetComponent<ObjectInteraction>();
                if (interactableObjectScript != null && !interactableObjectScript.IsOccupied())
                {
                    // 상호작용 가능한 오브젝트를 찾았을 경우, 타기 시도
                    photonView.RPC("EnterObjectRPC", RpcTarget.AllBuffered, collider.gameObject.GetPhotonView().ViewID);
                    break;
                }
            }
        }
    }

    [PunRPC]
    void EnterObjectRPC(int viewID)
    {
        // 네트워크를 통해 해당 오브젝트에 타도록 함
        GameObject obj = PhotonView.Find(viewID).gameObject;
        if (obj != null)
        {
            interactableObject = obj;
            isInteracting = true;

            // 타는 동작을 실행하지 않고 단순히 플레이어 위치를 오브젝트 위치로 이동시킴
            transform.position = obj.transform.position + Vector3.up * 2f;

            // 탈 수 있는 오브젝트를 플레이어 자식으로 만듦
            transform.SetParent(obj.transform);
        }
    }

    void TryExitObject()
    {
        // 탈출 시도
        photonView.RPC("ExitObjectRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ExitObjectRPC()
    {
        // 네트워크를 통해 오브젝트에서 탈출하도록 함
        if (interactableObject != null)
        {
            // 오브젝트에서 탈출 시 필요한 동작 수행
            // (애니메이션이 없으므로 단순히 플레이어를 원래 위치로 이동시킴)
            transform.position = interactableObject.transform.position + Vector3.up * 2f;

            // 탈 수 있는 오브젝트를 플레이어의 부모로 만듦
            transform.SetParent(null);

            isInteracting = false;
            interactableObject = null;
        }
    }
}
