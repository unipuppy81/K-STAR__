using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Trampoline : MonoBehaviourPun
{



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerTrampoline>().photonView.RPC("Jump", Photon.Pun.RpcTarget.All);
        }
    }

}
