using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCart : MonoBehaviourPunCallbacks
{
    private GameObject targetObject;
    private bool isOnObject = false;

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isOnObject)
                {
                    photonView.RPC("LeaveObject", RpcTarget.All);
                }
                else
                {
                    CheckForObjectToInteract();
                }
            }
        }
    }

    private void CheckForObjectToInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("InteractableObject"))
            {
                targetObject = hit.collider.gameObject;
                photonView.RPC("EnterObject", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void EnterObject()
    {
        // �÷��̾�� ������Ʈ�� ������ �ڽ����� ����
        transform.SetParent(targetObject.transform);
        transform.localPosition = Vector3.zero;

        // ������Ʈ�� ��ǥ �������� �̵��ϴ� �ڷ�ƾ ����
        StartCoroutine(MoveToObject());
    }

    [PunRPC]
    private void LeaveObject()
    {
        // �÷��̾�� ������Ʈ �θ� ����
        transform.SetParent(null);
        targetObject.transform.SetParent(null);

        // �̵� ���� �ڷ�ƾ ����
        StopAllCoroutines();

        isOnObject = false;
    }

    private IEnumerator MoveToObject()
    {
        isOnObject = true;

        // ������ ��ǥ ��ġ (���Ƿ� ����)
        Vector3 targetPosition = new Vector3(8f, -2f, 10f);

        while (Vector3.Distance(targetObject.transform.position, targetPosition) > 0.1f)
        {
            // ��ǥ �������� �̵�
            targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, Time.deltaTime * 5f);

            // �÷��̾ ������Ʈ�� �Բ� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 0.1f);

            yield return null;
        }

        // �̵��� �Ϸ�Ǹ� �ڵ����� ����
        photonView.RPC("LeaveObject", RpcTarget.All);
    }
}
