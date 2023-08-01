using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//īƮŸ������ ��ũ��Ʈ
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
                // �̹� ��ȣ�ۿ� ���̶�� Ż��
                TryExitObject();
            }
            else
            {
                // ��ȣ�ۿ� ���� �õ�
                TryEnterObject();
            }
        }
    }

    void TryEnterObject()
    {
        // �ֺ��� �ִ� InteractableObject�� Ȯ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("InteractableObject"))
            {
                ObjectInteraction interactableObjectScript = collider.GetComponent<ObjectInteraction>();
                if (interactableObjectScript != null && !interactableObjectScript.IsOccupied())
                {
                    // ��ȣ�ۿ� ������ ������Ʈ�� ã���� ���, Ÿ�� �õ�
                    photonView.RPC("EnterObjectRPC", RpcTarget.AllBuffered, collider.gameObject.GetPhotonView().ViewID);
                    break;
                }
            }
        }
    }

    [PunRPC]
    void EnterObjectRPC(int viewID)
    {
        // ��Ʈ��ũ�� ���� �ش� ������Ʈ�� Ÿ���� ��
        GameObject obj = PhotonView.Find(viewID).gameObject;
        if (obj != null)
        {
            interactableObject = obj;
            isInteracting = true;

            // Ÿ�� ������ �������� �ʰ� �ܼ��� �÷��̾� ��ġ�� ������Ʈ ��ġ�� �̵���Ŵ
            transform.position = obj.transform.position + Vector3.up * 2f;

            // Ż �� �ִ� ������Ʈ�� �÷��̾� �ڽ����� ����
            transform.SetParent(obj.transform);
        }
    }

    void TryExitObject()
    {
        // Ż�� �õ�
        photonView.RPC("ExitObjectRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ExitObjectRPC()
    {
        // ��Ʈ��ũ�� ���� ������Ʈ���� Ż���ϵ��� ��
        if (interactableObject != null)
        {
            // ������Ʈ���� Ż�� �� �ʿ��� ���� ����
            // (�ִϸ��̼��� �����Ƿ� �ܼ��� �÷��̾ ���� ��ġ�� �̵���Ŵ)
            transform.position = interactableObject.transform.position + Vector3.up * 2f;

            // Ż �� �ִ� ������Ʈ�� �÷��̾��� �θ�� ����
            transform.SetParent(null);

            isInteracting = false;
            interactableObject = null;
        }
    }
}
