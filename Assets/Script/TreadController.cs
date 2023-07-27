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
            Debug.Log("Tread Log");
            PlayerSteppedOn?.Invoke(photonView.OwnerActorNr);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)) { PlayerSteppedOn(1); }
        if (Input.GetKeyUp(KeyCode.O)) { PlayerSteppedOn(2); }
    }
}
