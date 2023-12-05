using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tree : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isGrounded;
    private bool isInteracting; // 상호작용 중인지 여부
    private bool isFrontPlayer; // 앞에 있는 플레이어인지 여부

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        isInteracting = false;
        isFrontPlayer = photonView.ViewID % 2 == 0; // ViewID를 이용하여 앞/뒤 플레이어 판별
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (!isInteracting)
            {
                // 상호작용 키(F) 입력 확인
                if (Input.GetKeyDown(KeyCode.F))
                {
                    photonView.RPC("StartInteraction", RpcTarget.All);
                }
            }
            else
            {
                // 상호작용 중인 경우
                if (isFrontPlayer)
                {
                    // 앞에 있는 플레이어는 좌우 이동만 가능
                    float horizontal = Input.GetAxis("Horizontal");
                    Vector3 movement = new Vector3(horizontal, 0f, 0f) * moveSpeed * Time.deltaTime;
                    transform.Translate(movement);
                }
                else
                {
                    // 뒤에 있는 플레이어는 상하 이동만 가능
                    float vertical = Input.GetAxis("Vertical");
                    Vector3 movement = new Vector3(0f, vertical, 0f) * moveSpeed * Time.deltaTime;
                    transform.Translate(movement);
                }

                // 플레이어 점프
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    photonView.RPC("Jump", RpcTarget.All);
                }

                // 상호작용 키(F)를 다시 누르면 상호작용 종료
                if (Input.GetKeyDown(KeyCode.F))
                {
                    photonView.RPC("StopInteraction", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void StartInteraction()
    {
        isInteracting = true;
    }

    [PunRPC]
    void StopInteraction()
    {
        isInteracting = false;
    }

    [PunRPC]
    void Jump()
    {
        // 동시에 높게 점프하는 로직을 추가
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
