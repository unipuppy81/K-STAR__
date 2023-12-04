using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRope : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        // PhotonView ������Ʈ ã��
        photonView = GetComponent<PhotonView>();

        // RopeManager ��ũ��Ʈ�� connectedPlayer�� �Ҵ�
        if (photonView != null)
        {
            // RopeManager ��ũ��Ʈ ��������
            RopeManager ropeManager = FindObjectOfType<RopeManager>();

            // RopeManager ��ũ��Ʈ�� �ִٸ� �� ���� connectedPlayer�� �Ҵ�
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