using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Seesaw : MonoBehaviourPunCallbacks
{
    private Rigidbody rigidBody;
    private float swingForce = 5f;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // 로컬 플레이어의 경우 키보드 입력을 사용하여 시소를 제어합니다.
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 swingDirection = new Vector3(horizontalInput, 0f, verticalInput);
            rigidBody.AddForce(swingDirection * swingForce);

            // 시소가 너무 빨리 회전하지 않도록 회전 각도를 제한합니다.
            float maxRotationAngle = 45f;
            float currentRotationAngle = Mathf.Clamp(transform.rotation.eulerAngles.z, -maxRotationAngle, maxRotationAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotationAngle);
        }
    }

    // 다른 플레이어들에게 시소의 상태를 동기화하는 메서드
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 시소 상태를 보냅니다.
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 원격 플레이어의 시소 상태를 받아 적용합니다.
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
