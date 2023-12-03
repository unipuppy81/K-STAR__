using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCart : MonoBehaviourPun
{
    //private bool isInteracting = false;

    //void Update()
    //{
    //    if (photonView.IsMine)
    //    {
    //        if (Input.GetKeyDown(KeyCode.F) && !isInteracting)
    //        {
    //            photonView.RPC("StartInteraction", RpcTarget.AllBuffered);
    //        }
    //    }
    //}

    //[PunRPC]
    //void StartInteraction()
    //{
    //    isInteracting = true;

    //    // 상호작용을 시작하면 InteractiveObject에 탑승을 요청하는 RPC를 호출
    //    if (TryGetInteractiveObject(out Cart interactiveObject))
    //    {
    //        interactiveObject.RequestEnterVehicle(photonView.ViewID);
    //    }
    //}

    //bool TryGetInteractiveObject(out Cart interactiveObject)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
    //    {
    //        interactiveObject = hit.collider.GetComponent<Cart>();
    //        return interactiveObject != null;
    //    }

    //    interactiveObject = null;
    //    return false;
    //}
}
