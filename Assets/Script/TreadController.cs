using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

// TreadController.cs
public class TreadController : MonoBehaviourPun
{
    public static event Action<int> PlayerSteppedOn;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerSteppedOn?.Invoke(photonView.OwnerActorNr);
        }
    }
}
