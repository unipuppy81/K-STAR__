using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OjingU : MonoBehaviour
{
    public float moveAmount = 3f;
    private bool isPlayerOnPlatform = false;
    private PhotonView photonView;
    public GameObject block;

    private float elapsedTime = 0f;
    private bool canMove = true;

    private void Start()
    {
        // PhotonView를 참조할 변수에 할당
        photonView = GetComponent<PhotonView>();

        // PhotonView가 없다면 경고 출력
        if (photonView == null)
        {
            Debug.LogError("PhotonView is not found on this GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerOnPlatform = true;

            // Photon 네트워크를 통해 상태 동기화
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);

            StartCoroutine(MoveBlock());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;

            // Photon 네트워크를 통해 상태 동기화
            photonView.RPC("SyncPlatformState", RpcTarget.All, false);

            // ResetPlatform 메서드 호출
            StartCoroutine(ResetPlatform());
        }
    }

    private void Update()
    {
        /*if (isPlayerOnPlatform && photonView.IsMine)
        {
            elapsedTime += Time.deltaTime;

            if (canMove && elapsedTime < 1f)
            {
                MovePlatform();
            }
            else
            {
                canMove = false;
                // 1초가 지나면 canMove를 false로 설정하여 이후의 이동을 막습니다.
            }
        }*/
    }

    private void MovePlatform()
    {
        block.transform.Translate(Vector3.up * moveAmount * Time.deltaTime);

        // Photon 네트워크를 통해 위치 동기화
        photonView.RPC("SyncPlatformPosition", RpcTarget.Others, transform.position);
    }

    private IEnumerator MoveBlock()
    {
        float resetDuration = 1f;
        float timer = 0f;

        while (timer < resetDuration)
        {
            block.transform.Translate(Vector3.up * moveAmount * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        canMove = true;
        elapsedTime = 0f;
    }

    private IEnumerator ResetPlatform()
    {
        // 충돌이 끝나면 호출되어 1초 동안 y축으로 -3만큼 이동하고 canMove를 true로 설정하여 이후의 이동을 가능하게 합니다.
        float resetDuration = 1f;
        float timer = 0f;

        while (timer < resetDuration)
        {
            block.transform.Translate(Vector3.down * moveAmount * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        canMove = true;
        elapsedTime = 0f;
    }

    [PunRPC]
    private void SyncPlatformState(bool isPlayerOn)
    {
        isPlayerOnPlatform = isPlayerOn;
    }

    [PunRPC]
    private void SyncPlatformPosition(Vector3 newPosition)
    {
        if (!photonView.IsMine)
        {
            block.transform.position = newPosition;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송할 때의 코드
            stream.SendNext(isPlayerOnPlatform);
            stream.SendNext(block.transform.position);
        }
        else
        {
            // 데이터를 수신할 때의 코드
            if (stream.Count > 0)
            {
                isPlayerOnPlatform = (bool)stream.ReceiveNext();
            }

            if (stream.Count > 0)
            {
                block.transform.position = (Vector3)stream.ReceiveNext();
            }
        }
    }
}