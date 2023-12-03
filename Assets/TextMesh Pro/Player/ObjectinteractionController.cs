using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectinteractionController : MonoBehaviourPunCallbacks
{
    private bool isHoldingObject = false; // ��ü�� ��� �ִ��� ���θ� ��Ÿ���� ����
    private GameObject heldObject = null; // ��� �ִ� ��ü�� �����ϴ� ����

    private float interactionRange = 1f; // ��ȣ�ۿ� ������ �Ÿ�

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // ��ȣ�ۿ� ���
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
            // ��� �ִ� ��ü�� �÷��̾��� ������ �̵�
            heldObject.transform.position = transform.position + transform.forward * 2f;
        }
    }

    void TryPickUpObject()
    {
        // �ֺ��� �ִ� ��ü���� �����ɴϴ�.
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (Collider collider in nearbyColliders)
        {
            // ��ü�� "Pickable" �±װ� �����Ǿ� �־�� ��ȣ�ۿ� �����մϴ�.
            if (collider.CompareTag("Pickable"))
            {
                // ��ü�� ���ø��ϴ�.
                photonView.RPC("PickUpObjectRPC", RpcTarget.AllBuffered, collider.gameObject.GetPhotonView().ViewID);

                // ��ȣ�ۿ� �ߴ�
                break;
            }
        }
    }

    [PunRPC]
    void PickUpObjectRPC(int viewID)
    {
        // ��Ʈ��ũ�� ���� ��ü�� ���ø��ϴ�.
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
        // ��ü�� �����ϴ�.
        photonView.RPC("DropObjectRPC", RpcTarget.AllBuffered);

        // ��ȣ�ۿ� �ߴ�
        isHoldingObject = false;
    }

    [PunRPC]
    void DropObjectRPC()
    {
        // ��Ʈ��ũ�� ���� ��ü�� �����ϴ�.
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
        }
    }
}
