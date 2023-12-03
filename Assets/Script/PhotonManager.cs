using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "k-star";

    private void Awake()
    {
        // 같은 룸의 유저들에게 자동을 씬을 로딩
        //PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전끼리만 접속
        PhotonNetwork.GameVersion = gameVersion;
        
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;

        // 포톤 서버와 통신 횟수 설정. 기본 30회
        //PhotonNetwork.SendRate
        
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        Debug.Log("00.포톤 매니저 시작");
        PhotonNetwork.NickName = userId;
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. 포톤 서버에 접속");
        // PhotonNetwork.InLobby = 로비에 있는지 bool
        // PhotonNetwork.JoinLobby(); // 로비 입장

        
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        // PhotonNetwork.InLobby = 로비에 있는지 bool
        PhotonNetwork.JoinRandomRoom(); // 랜덤 매치메이킹
    }

   

    // 랜덤 룸 입장 실패 시 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // returnCode : 오류 코드
        // message : 오류 설명
       
        Debug.Log("02.랜덤 룸 접속 실패");

        //룸 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true; // 로비에서 룸 노출 여부
        ro.MaxPlayers = 10;

        //룸을 생성-> 자동 입장
        PhotonNetwork.CreateRoom("room1", ro);
    }


    // 룸 생성 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("03.방 생성 완료");
        // PhotonNetwork.CurrentRoom.Name : 현재 방 이름
    }

    public override void OnJoinedRoom()
    {
        // PhotonNetwork.InRoom : 룸에 있는지 여부 bool
        // PhotonNetwork.CurrentRoom.PlayerCount : 현재 방 인원 수
        
        Debug.Log("04. 방 입장 완료");
        GameManager.instance.isConnect = true;
    
        // 방 접속 사용자 정보 확인
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        // 캐릭터 출현 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int index = Random.Range(0, Random.Range(0,points.Length));

        // 캐릭터를 생성
        PhotonNetwork.Instantiate("Player_01", points[index].position, points[index].rotation, 0);
       
    }
    
}
