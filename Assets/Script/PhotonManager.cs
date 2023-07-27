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
        // ���� ���� �����鿡�� �ڵ��� ���� �ε�
        //PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ���������� ����
        PhotonNetwork.GameVersion = gameVersion;
        
        // ���� ���̵� �Ҵ�
        PhotonNetwork.NickName = userId;

        // ���� ������ ��� Ƚ�� ����. �⺻ 30ȸ
        //PhotonNetwork.SendRate
        
        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        Debug.Log("00.���� �Ŵ��� ����");
        PhotonNetwork.NickName = userId;
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. ���� ������ ����");
        // PhotonNetwork.InLobby = �κ� �ִ��� bool
        // PhotonNetwork.JoinLobby(); // �κ� ����

        
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        // PhotonNetwork.InLobby = �κ� �ִ��� bool
        PhotonNetwork.JoinRandomRoom(); // ���� ��ġ����ŷ
    }

   

    // ���� �� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // returnCode : ���� �ڵ�
        // message : ���� ����
       
        Debug.Log("02.���� �� ���� ����");

        //�� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true; // �κ񿡼� �� ���� ����
        ro.MaxPlayers = 10;

        //���� ����-> �ڵ� ����
        PhotonNetwork.CreateRoom("room1", ro);
    }


    // �� ���� �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("03.�� ���� �Ϸ�");
        // PhotonNetwork.CurrentRoom.Name : ���� �� �̸�
    }

    public override void OnJoinedRoom()
    {
        // PhotonNetwork.InRoom : �뿡 �ִ��� ���� bool
        // PhotonNetwork.CurrentRoom.PlayerCount : ���� �� �ο� ��
        
        Debug.Log("04. �� ���� �Ϸ�");
        GameManager.instance.isConnect = true;
    
        // �� ���� ����� ���� Ȯ��
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        // ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int index = Random.Range(0, Random.Range(0,points.Length));

        // ĳ���͸� ����
        PhotonNetwork.Instantiate("PlayerArmature", points[index].position, points[index].rotation, 0);
    }
}
