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
        // PhotonView�� ������ ������ �Ҵ�
        photonView = GetComponent<PhotonView>();

        // PhotonView�� ���ٸ� ��� ���
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

            // Photon ��Ʈ��ũ�� ���� ���� ����ȭ
            photonView.RPC("SyncPlatformState", RpcTarget.All, true);

            StartCoroutine(MoveBlock());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;

            // Photon ��Ʈ��ũ�� ���� ���� ����ȭ
            photonView.RPC("SyncPlatformState", RpcTarget.All, false);

            // ResetPlatform �޼��� ȣ��
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
                // 1�ʰ� ������ canMove�� false�� �����Ͽ� ������ �̵��� �����ϴ�.
            }
        }*/
    }

    private void MovePlatform()
    {
        block.transform.Translate(Vector3.up * moveAmount * Time.deltaTime);

        // Photon ��Ʈ��ũ�� ���� ��ġ ����ȭ
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
        // �浹�� ������ ȣ��Ǿ� 1�� ���� y������ -3��ŭ �̵��ϰ� canMove�� true�� �����Ͽ� ������ �̵��� �����ϰ� �մϴ�.
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
            // �����͸� ������ ���� �ڵ�
            stream.SendNext(isPlayerOnPlatform);
            stream.SendNext(block.transform.position);
        }
        else
        {
            // �����͸� ������ ���� �ڵ�
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