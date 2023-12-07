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

            // F 키를 눌렀을 때 상호작용
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
        // F 키를 눌렀을 때 수행할 상호작용 코드 추가

        // 여기에 다른 플레이어의 위치를 변경하는 코드 추가
        if (photonView.IsMine && transform.position.z > 0)
        {
            // 예시: 다른 플레이어를 현재 위치에서 오른쪽으로 오브젝트 앞쪽에 위치하도록 조절
            Vector3 objectForward = transform.forward; // 오브젝트의 정면 방향을 가져옴
            Vector3 newPosition = transform.position + objectForward * 2f; // 현재 위치에서 정면 방향으로 2만큼 이동
            photonView.RPC("MoveOtherPlayer", RpcTarget.AllBuffered, newPosition);
        }
        else if (photonView.IsMine && transform.position.z < 0)
        {
            // 예시: 다른 플레이어를 현재 위치에서 왼쪽으로 오브젝트 뒤쪽에 위치하도록 조절
            Vector3 objectBackward = -transform.forward; // 오브젝트의 뒷면 방향을 가져옴
            Vector3 newPosition = transform.position + objectBackward * 2f; // 현재 위치에서 뒷면 방향으로 2만큼 이동
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

