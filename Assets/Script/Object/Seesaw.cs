using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Seesaw : MonoBehaviourPunCallbacks , IPunObservable
{

    //seesawobject라는 빈 오브젝트에 포톤 뷰 추가하기
    private Vector3 latestPosition;
    private Quaternion latestRotation;
    private GameObject swingObject;

    private void Start()
    {
        // 동기화할 빈 GameObject 생성
        swingObject = new GameObject("SeesawObject");
        swingObject.transform.SetParent(transform);

        // 빈 GameObject에 포톤 뷰 추가
        PhotonView photonView = swingObject.AddComponent<PhotonView>();
        photonView.ViewID = PhotonNetwork.AllocateViewID();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // 동기화된 위치와 회전을 물체에 적용
            transform.position = Vector3.Lerp(transform.position, latestPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRotation, Time.deltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 데이터를 보냅니다.
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 다른 플레이어의 데이터를 수신합니다.
            latestPosition = (Vector3)stream.ReceiveNext();
            latestRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
