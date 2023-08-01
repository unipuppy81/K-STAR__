using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectinteractionController : MonoBehaviourPunCallbacks
{
    private bool isHoldingObject = false; // 물체를 들고 있는지 여부를 나타내는 변수
    private GameObject heldObject = null; // 들고 있는 물체를 저장하는 변수

    private float interactionRange = 1f; // 상호작용 가능한 거리

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // 상호작용 토글
            if (isHoldingObject)
            {
                DropObject();
            }
            else
            {
                TryPickUpObject();
            }
        }

        if (isHoldingObject && heldObject != null)
        {
            // 들고 있는 물체를 플레이어의 앞으로 이동
            heldObject.transform.position = transform.position + transform.forward * 2f;
        }
    }

    void TryPickUpObject()
    {
        // 주변에 있는 물체들을 가져옵니다.
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (Collider collider in nearbyColliders)
        {
            // 물체에 "Pickable" 태그가 지정되어 있어야 상호작용 가능합니다.
            if (collider.CompareTag("Pickable"))
            {
                // 물체를 들어올립니다.
                photonView.RPC("PickUpObjectRPC", RpcTarget.AllBuffered, collider.gameObject.GetPhotonView().ViewID);

                // 상호작용 중단
                break;
            }
        }
    }

    [PunRPC]
    void PickUpObjectRPC(int viewID)
    {
        // 네트워크를 통해 물체를 들어올립니다.
        GameObject obj = PhotonView.Find(viewID).gameObject;
        if (obj != null)
        {
            heldObject = obj;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            isHoldingObject = true;
        }
    }

    void DropObject()
    {
        // 물체를 놓습니다.
        photonView.RPC("DropObjectRPC", RpcTarget.AllBuffered);

        // 상호작용 중단
        isHoldingObject = false;
    }

    [PunRPC]
    void DropObjectRPC()
    {
        // 네트워크를 통해 물체를 놓습니다.
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
        }
    }
}
