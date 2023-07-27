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

    private void OnEnable()
    {
        TreadController.PlayerSteppedOn += OnPlayerStep;
    }

    private void OnDisable()
    {
        TreadController.PlayerSteppedOn -= OnPlayerStep;
    }

    void OnPlayerStep(int playerActorNumber)
    {
        if (playerActorNumber == 1)
        {
            player1Step = 1;
        }
        else if (playerActorNumber == 2)
        {
            player2Step = 1;
        }

        if (player1Step == 1 && player2Step == 1)
        {
            photonView.RPC("MoveWall", RpcTarget.All);
            player1Step = 0;
            player2Step = 0;
        }
    }

    [PunRPC]
    void MoveWall()
    {
        wall.transform.position += moveAmount;
    }
}
