using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRope : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        // PhotonView 컴포넌트 찾기
        photonView = GetComponent<PhotonView>();

        // RopeManager 스크립트의 connectedPlayer에 할당
        if (photonView != null)
        {
            // RopeManager 스크립트 가져오기
            RopeManager ropeManager = FindObjectOfType<RopeManager>();

            // RopeManager 스크립트가 있다면 그 안의 connectedPlayer에 할당
            if (ropeManager != null)
            {
                ropeManager.connectedPlayer = photonView.transform;
            }
            else
            {
                Debug.LogError("RopeManager script not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("PhotonView not found on this GameObject.");
        }
    }
}