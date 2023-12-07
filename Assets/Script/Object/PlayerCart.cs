using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCart : MonoBehaviourPunCallbacks
{
    private GameObject targetObject;
    private bool isOnObject = false;


    // ���������� ������ ������
    private Vector3 player1Destination = new Vector3(-86f, 1.5f, -73.5f);
    private Vector3 player2Destination = new Vector3(-86f, 1.5f, -83.5f);

    public float fuck;

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
            Debug.Log("Raycast : " + hit.collider.gameObject.name);
            Debug.Log("TTTT");
            targetObject = hit.collider.gameObject;
            photonView.RPC("EnterObject", RpcTarget.All);
            if (hit.collider.tag == "InteractableObject")
            {

            }
        }
    }

    [PunRPC]
    private void EnterObject()
    {
        // �÷��̾�� ������Ʈ�� ������ �ڽ����� ����
        transform.SetParent(targetObject.transform);
        transform.localPosition = Vector3.zero;

        // �� �÷��̾ ���� �������� ����
        Vector3 targetPosition = (photonView.Owner.ActorNumber == 1) ? player1Destination : player2Destination;

        // ������Ʈ�� ��ǥ �������� �̵��ϴ� �ڷ�ƾ ����
        StartCoroutine(MoveToObject(targetPosition));
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

    private IEnumerator MoveToObject(Vector3 targetPosition)
    {
        isOnObject = true;

        while (Vector3.Distance(targetObject.transform.position, targetPosition) > 0.1f)
        {
            // ��ǥ �������� �̵�
            targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, Time.deltaTime * 5f);

            // �÷��̾ ������Ʈ�� �Բ� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * fuck);

            yield return null;
        }

        // �̵��� �Ϸ�Ǹ� �ڵ����� ����
        photonView.RPC("LeaveObject", RpcTarget.All);
    }
}
