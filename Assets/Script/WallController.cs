using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WallController : MonoBehaviourPun
{
    public GameObject wall;
    public Vector3 moveAmount;
    private int player1Step = 0;
    private int player2Step = 0;
    PhotonView pv;

    private void Start()
    {
        pv = photonView;
        //pv = GameObject.Find("Door").GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        Debug.Log("Wall OnEnabled");
        TreadController.PlayerSteppedOn += OnPlayerStep;
        //TreadController.PlayerSteppedOn += (T) => OnPlayerStep(1);
    }

    private void OnDisable()
    {
        TreadController.PlayerSteppedOn -= OnPlayerStep;
    }

    void OnPlayerStep(int playerActorNumber)
    {
        int a = playerActorNumber;

        if (a == 1)
        {
            player1Step = 1;
            Debug.Log("A1");
        }
        else if (a == 2)
        {
            player2Step = 1;
            Debug.Log("A2");
        }

        if (player1Step == 1 && player2Step == 1)
        {
            //wall.transform.position += moveAmount;
            MoveWall2();
            player1Step = 0;
            player2Step = 0;
        }
    }

    [PunRPC]
    protected virtual void MoveWall()
    {
        Debug.Log("MoveWall Input");
        //wall.transform.position += moveAmount;
    }


    void MoveWall2()
    {
        Debug.Log("MoveWall 2 Input");
        wall.transform.position += moveAmount;
    }


}
