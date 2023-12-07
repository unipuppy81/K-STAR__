using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCart : MonoBehaviourPunCallbacks
{
    private GameObject targetObject;
    private bool isOnObject = false;


    // 도착지점을 저장할 변수들
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
        // 플레이어와 오브젝트를 서로의 자식으로 설정
        transform.SetParent(targetObject.transform);
        transform.localPosition = Vector3.zero;

        // 각 플레이어에 따른 도착지점 설정
        Vector3 targetPosition = (photonView.Owner.ActorNumber == 1) ? player1Destination : player2Destination;

        // 오브젝트가 목표 지점까지 이동하는 코루틴 시작
        StartCoroutine(MoveToObject(targetPosition));
    }

    [PunRPC]
    private void LeaveObject()
    {
        // 플레이어와 오브젝트 부모 해제
        transform.SetParent(null);
        targetObject.transform.SetParent(null);

        // 이동 중인 코루틴 중지
        StopAllCoroutines();

        isOnObject = false;
    }

    private IEnumerator MoveToObject(Vector3 targetPosition)
    {
        isOnObject = true;

        while (Vector3.Distance(targetObject.transform.position, targetPosition) > 0.1f)
        {
            // 목표 지점까지 이동
            targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, Time.deltaTime * 5f);

            // 플레이어도 오브젝트와 함께 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * fuck);

            yield return null;
        }

        // 이동이 완료되면 자동으로 하차
        photonView.RPC("LeaveObject", RpcTarget.All);
    }
}
