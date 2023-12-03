using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerTrampoline : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpPad"))
        {
            photonView.RPC("Jump", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Jump()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
