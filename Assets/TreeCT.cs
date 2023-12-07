using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class TreeCT : MonoBehaviourPunCallbacks, IPunObservable
{
    //private CharacterController characterController;
    private Vector3 moveDirection;
    private bool isGrounded;
    private float verticalVelocity;
    private float jumpForce = 5.0f;

    private void Start()
    {
       // characterController = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            HandleInput();

            // F Ű�� ������ �� ��ȣ�ۿ�
            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            SmoothMove();
        }
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (photonView.IsMine && transform.position.z > 0)
        {
            moveDirection = new Vector3(horizontal, 0f, 0f);
        }
        else if (photonView.IsMine && transform.position.z < 0)
        {
            moveDirection = new Vector3(0f, 0f, vertical);
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        moveDirection.y = verticalVelocity;

       // characterController.Move(moveDirection * Time.deltaTime);
    }

    [PunRPC]
    private void Interact()
    {
        // F Ű�� ������ �� ������ ��ȣ�ۿ� �ڵ� �߰�

        // ���⿡ �ٸ� �÷��̾��� ��ġ�� �����ϴ� �ڵ� �߰�
        if (photonView.IsMine && transform.position.z > 0)
        {
            // ����: �ٸ� �÷��̾ ���� ��ġ���� ���������� ������Ʈ ���ʿ� ��ġ�ϵ��� ����
            Vector3 objectForward = transform.forward; // ������Ʈ�� ���� ������ ������
            Vector3 newPosition = transform.position + objectForward * 2f; // ���� ��ġ���� ���� �������� 2��ŭ �̵�
            photonView.RPC("MoveOtherPlayer", RpcTarget.AllBuffered, newPosition);
        }
        else if (photonView.IsMine && transform.position.z < 0)
        {
            // ����: �ٸ� �÷��̾ ���� ��ġ���� �������� ������Ʈ ���ʿ� ��ġ�ϵ��� ����
            Vector3 objectBackward = -transform.forward; // ������Ʈ�� �޸� ������ ������
            Vector3 newPosition = transform.position + objectBackward * 2f; // ���� ��ġ���� �޸� �������� 2��ŭ �̵�
            photonView.RPC("MoveOtherPlayer", RpcTarget.AllBuffered, newPosition);
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
    }

    #region IPunObservable implementation

    private Vector3 realPosition = Vector3.zero;
    private Quaternion realRotation = Quaternion.identity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    #endregion

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    isGrounded = characterController.isGrounded;
    //}
}

