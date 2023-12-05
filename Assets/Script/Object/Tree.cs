using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tree : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isGrounded;
    private bool isInteracting; // ��ȣ�ۿ� ������ ����
    private bool isFrontPlayer; // �տ� �ִ� �÷��̾����� ����

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        isInteracting = false;
        isFrontPlayer = photonView.ViewID % 2 == 0; // ViewID�� �̿��Ͽ� ��/�� �÷��̾� �Ǻ�
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (!isInteracting)
            {
                // ��ȣ�ۿ� Ű(F) �Է� Ȯ��
                if (Input.GetKeyDown(KeyCode.F))
                {
                    photonView.RPC("StartInteraction", RpcTarget.All);
                }
            }
            else
            {
                // ��ȣ�ۿ� ���� ���
                if (isFrontPlayer)
                {
                    // �տ� �ִ� �÷��̾�� �¿� �̵��� ����
                    float horizontal = Input.GetAxis("Horizontal");
                    Vector3 movement = new Vector3(horizontal, 0f, 0f) * moveSpeed * Time.deltaTime;
                    transform.Translate(movement);
                }
                else
                {
                    // �ڿ� �ִ� �÷��̾�� ���� �̵��� ����
                    float vertical = Input.GetAxis("Vertical");
                    Vector3 movement = new Vector3(0f, vertical, 0f) * moveSpeed * Time.deltaTime;
                    transform.Translate(movement);
                }

                // �÷��̾� ����
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    photonView.RPC("Jump", RpcTarget.All);
                }

                // ��ȣ�ۿ� Ű(F)�� �ٽ� ������ ��ȣ�ۿ� ����
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
        // ���ÿ� ���� �����ϴ� ������ �߰�
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
