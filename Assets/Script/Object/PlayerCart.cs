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

    //    // ��ȣ�ۿ��� �����ϸ� InteractiveObject�� ž���� ��û�ϴ� RPC�� ȣ��
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
